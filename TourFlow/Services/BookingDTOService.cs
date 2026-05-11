using Microsoft.AspNetCore.Components.Forms;
using System.Text;
using System.Text.Json;
using TourFlow.Client;
using TourFlow.Client.Enums;
using TourFlow.Client.Interfaces;
using TourFlow.Client.Models;
using TourFlow.Data;
using TourFlow.Interfaces;
using TourFlow.Models;

namespace TourFlow.Services
{
    public class BookingDTOService(IBookingRepository repository, IConfiguration config) : IBookingDTOService
    {
        public async Task<string> GenerateItineraryWithAIAsync(EnquiryDTO enquiry)
        {
            if (enquiry == null) return string.Empty;

            var apiKey = config["Grok:ApiKey"];
            var model = config["Grok:Model"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return "Grok API key is not configured.";
            }

            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var prompt = $@"
                    You are an expert luxury travel itinerary writer for Vosaio.

                    Generate a compelling, professional itinerary summary for this group trip:

                    - Destination: {enquiry.Destination}
                    - Group Size: {enquiry.GroupSize} travelers
                    - Dates: {enquiry.StartDate?.ToString("dd MMMM yyyy")} to {enquiry.EndDate?.ToString("dd MMMM yyyy")}
                    - Budget: {enquiry.Budget}
                    - Requested Services: {enquiry.RequestedServices ?? "Not specified"}
                    - Additional Notes: {enquiry.AdditionalNotes ?? "None"}

                    Requirements:
                    - Warm, engaging, and professional tone.
                    - Highlight key experiences and why this trip will be memorable.
                    - Keep total length between 160-240 words.
                    - No markdown formatting.";

                var requestBody = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful and creative senior travel planner." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.75,
                    max_tokens = 800
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.x.ai/v1/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Grok API Error: {response.StatusCode} - {error}");
                    return "Sorry, AI generation failed. Please write manually.";
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GrokResponse>(responseString);
                var message = result?.Choices?.FirstOrDefault()?.Message;
                var generatedText = message?.Content?.Trim();

                if (string.IsNullOrWhiteSpace(generatedText))
                    generatedText = message?.ReasoningContent?.Trim();

                return generatedText ?? "Unable to generate itinerary at this moment.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Grok API Exception: {ex.Message}");
                return "Sorry, we couldn't generate the itinerary right now. Please write it manually.";
            }
        }

        public async Task<BookingDTO?> GetBookingByIdAsync(int bookingId)
        {
            Booking? booking = await repository.GetBookingByIdAsync(bookingId);

            if (booking is null)
            {
                return null;
            }

            return booking?.ToDTO();
        }

        public async Task<BookingDTO> CreateBookingAsync(BookingDTO dto, UserInfo user, IBrowserFile file)
        {
            Booking dbBooking = new()
            {
                EnquiryId = dto.EnquiryId,
                Description = dto.Description,
                TotalAmount = dto.TotalAmount,
                Currency = dto.Currency,
                TourProvider = dto.TourProvider,
                Status = BookingStatus.InProgress,
                CreatedById = user.UserId,
                Created = DateTimeOffset.UtcNow
            };

            dbBooking = await repository.CreateBookingAsync(dbBooking, user, file);

            Console.WriteLine("******** EMAIL SERVICE ********");
            Console.WriteLine($"You have received a new quotation from Tour Flow");
            Console.WriteLine($"A new enquiry email has been sent to travel agent");
            Console.WriteLine("******** EMAIL SERVICE ********");

            return dbBooking.ToDTO();
        }
        
        public async Task<IEnumerable<BookingDTO>> GetAllBookingsAsync(UserInfo user)
        {
            IEnumerable<Booking> allBookings = await repository.GetAllBookingsAsync(user);
            return allBookings.Select(b => b.ToDTO());
        }

        public async Task CancelBookingAsync(int bookingId, UserInfo user)
        {
            await repository.CancelBookingAsync(bookingId, user);
        }

        public async Task ConfirmBookingAsync(int bookingId, UserInfo user)
        {
            await repository.ConfirmBookingAsync(bookingId, user);
        }
    }
}
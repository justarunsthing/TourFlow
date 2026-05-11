using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using TourFlow.Client;
using TourFlow.Client.Enums;
using TourFlow.Client.Interfaces;
using TourFlow.Client.Models;
using TourFlow.Data;
using TourFlow.Interfaces;
using TourFlow.Models;

namespace TourFlow.Services
{
    public class BookingDTOService(IBookingRepository repository) : IBookingDTOService
    {
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
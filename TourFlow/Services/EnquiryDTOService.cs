using Microsoft.AspNetCore.Identity.UI.Services;
using TourFlow.Client;
using TourFlow.Client.Interfaces;
using TourFlow.Client.Models;
using TourFlow.Interfaces;
using TourFlow.Models;

namespace TourFlow.Services
{
    public class EnquiryDTOService(IEnquiryRepository repository, IEmailSender emailSender) : IEnquiryDTOService
    {
        public async Task<EnquiryDTO?> GetEnquiryByIdAsync(int enquiryId)
        {
            Enquiry? enquiry = await repository.GetEnquiryByIdAsync(enquiryId);

            return enquiry?.ToDTO();
        }

        public async Task<IEnumerable<EnquiryDTO>> GetAllEnquiriesAsync(UserInfo user)
        {
            IEnumerable<Enquiry> allEnquiries = await repository.GetAllEnquiriesAsync(user);

            return allEnquiries.Select(e => e.ToDTO());
        }

        public async Task<EnquiryDTO> CreateEnquiryAsync(EnquiryDTO enquiry, UserInfo user)
        {
            Enquiry dbEnquiry = new()
            {
                GroupSize = enquiry.GroupSize,
                Destination = enquiry.Destination,
                Budget = enquiry.Budget,
                Created = DateTimeOffset.UtcNow,
                StartDate = (DateTimeOffset)enquiry.StartDate!,
                EndDate = (DateTimeOffset)enquiry.EndDate!,
                RequestedServices = enquiry.RequestedServices,
                AdditionalNotes = enquiry.AdditionalNotes
            };

            dbEnquiry = await repository.CreateEnquiryAsync(dbEnquiry, user);

            try
            {
                await emailSender.SendEmailAsync("admin.tourflow@mailinator.com", "New enquiry", $"You have received a new enquiry from travel agent with id: {user.TravelAgentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                Console.WriteLine("******** EMAIL SERVICE ********");
                Console.WriteLine($"You have received a new enquiry from travel agent with id: {user.TravelAgentId}");
                Console.WriteLine($"A new enquiry email has been sent to admin.tourflow@mailinator.com");
                Console.WriteLine("******** EMAIL SERVICE ********");
            }

            return dbEnquiry.ToDTO();
        }

        public async Task SetEnquiryToQuotedAsync(int enquiryId, UserInfo user)
        {
            await repository.SetEnquiryToQuotedAsync(enquiryId, user);
        }
    }
}
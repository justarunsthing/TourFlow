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
        public async Task<IEnumerable<EnquiryDTO>> GetAllEnquiriesAsync()
        {
            IEnumerable<Enquiry> tourEnquiries = await repository.GetAllEnquiriesAsync();
            IEnumerable<EnquiryDTO> tourEnquiryDTOs = tourEnquiries.Select(e => e.ToDTO());

            return tourEnquiryDTOs;
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
                await emailSender.SendEmailAsync("punarun@hotmail.com", "New enquiry", $"You have received a new enquiry from travel agent with id: {user.TravelAgentId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dbEnquiry.ToDTO();
        }
    }
}
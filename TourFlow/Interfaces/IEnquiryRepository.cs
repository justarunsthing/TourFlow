using TourFlow.Client;
using TourFlow.Models;

namespace TourFlow.Interfaces
{
    public interface IEnquiryRepository
    {
        Task<IEnumerable<Enquiry>> GetAllEnquiriesAsync();
        Task<Enquiry> CreateEnquiryAsync(Enquiry enquiry, UserInfo user);
    }
}
using TourFlow.Client;
using TourFlow.Models;

namespace TourFlow.Interfaces
{
    public interface IEnquiryRepository
    {
        Task<Enquiry?> GetEnquiryByIdAsync(int id);
        Task<IEnumerable<Enquiry>> GetAllEnquiriesAsync(UserInfo user);
        Task<Enquiry> CreateEnquiryAsync(Enquiry enquiry, UserInfo user);
        Task UpdateEnquiryAsync(int enquiryId, UserInfo user);
    }
}
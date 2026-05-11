using TourFlow.Client.Models;

namespace TourFlow.Client.Interfaces
{
    public interface IEnquiryDTOService
    {
        Task<EnquiryDTO?> GetEnquiryByIdAsync(int enquiryId);
        Task<IEnumerable<EnquiryDTO>> GetAllEnquiriesAsync(UserInfo user);
        Task<EnquiryDTO> CreateEnquiryAsync(EnquiryDTO enquiry, UserInfo user);
        Task SetEnquiryToQuotedAsync(int enquiryId, UserInfo user);
    }
}
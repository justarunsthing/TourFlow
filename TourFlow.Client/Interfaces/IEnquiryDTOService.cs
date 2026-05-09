using TourFlow.Client.Models;

namespace TourFlow.Client.Interfaces
{
    public interface IEnquiryDTOService
    {
        Task<IEnumerable<EnquiryDTO>> GetAllEnquiriesAsync();
    }
}
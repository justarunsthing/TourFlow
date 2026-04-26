using TourFlow.Models;

namespace TourFlow.Interfaces
{
    public interface IEnquiryRepository
    {
        Task<IEnumerable<TourEnquiry>> GetAllEnquiriesAsync();
    }
}
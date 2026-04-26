using TourFlow.Models;
using TourFlow.Interfaces;
using TourFlow.Client.Models;
using TourFlow.Client.Interfaces;

namespace TourFlow.Services
{
    public class EnquiryDTOService(IEnquiryRepository repository) : IEnquiryDTOService
    {
        public async Task<IEnumerable<TourEnquiryDTO>> GetAllEnquiriesAsync()
        {
            IEnumerable<TourEnquiry> tourEnquiries = await repository.GetAllEnquiriesAsync();
            IEnumerable<TourEnquiryDTO> tourEnquiryDTOs = tourEnquiries.Select(e => e.ToDTO());

            return tourEnquiryDTOs;
        }
    }
}

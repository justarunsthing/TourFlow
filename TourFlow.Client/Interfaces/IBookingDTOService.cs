using TourFlow.Client.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace TourFlow.Client.Interfaces
{
    public interface IBookingDTOService
    {
        Task<BookingDTO> CreateBookingAsync(BookingDTO dto, UserInfo user, IBrowserFile file);
    }
}
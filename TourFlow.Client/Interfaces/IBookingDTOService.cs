using TourFlow.Client.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace TourFlow.Client.Interfaces
{
    public interface IBookingDTOService
    {
        Task<BookingDTO?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<BookingDTO>> GetAllBookingsAsync(UserInfo user);
        Task<BookingDTO> CreateBookingAsync(BookingDTO dto, UserInfo user, IBrowserFile file);
        Task CancelBookingAsync(int bookingId, UserInfo user);
        Task ConfirmBookingAsync(int bookingId, UserInfo user);
    }
}
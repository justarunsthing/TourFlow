using Microsoft.AspNetCore.Components.Forms;
using TourFlow.Client;
using TourFlow.Models;

namespace TourFlow.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync(UserInfo user);
        Task<Booking> CreateBookingAsync(Booking booking, UserInfo user, IBrowserFile file);
        Task CancelBookingAsync(int bookingId, UserInfo user);
        Task ConfirmBookingAsync(int bookingId, UserInfo user);
    }
}
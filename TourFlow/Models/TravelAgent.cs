using TourFlow.Data;

namespace TourFlow.Models
{
    public class TravelAgent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        // Navigational Properties
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Enquiry> Enquiries { get; set; } = [];
        public virtual ICollection<Booking> Bookings { get; set; } = [];
    }
}
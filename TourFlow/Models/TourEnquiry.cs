using TourFlow.Data;
using TourFlow.Client.Enums;

namespace TourFlow.Models
{
    public class TourEnquiry
    {
        public int Id { get; set; }
        public string? EnquiryNumber { get; set; } 
        public int GroupSize { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string? Destination { get; set; }
        public string? Budget { get; set; }
        public string? RequestedServices { get; set; }
        public string? AdditionalNotes { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public EnquiryStatus Status { get; set; } = EnquiryStatus.New;

        // Navigation properties
        public int TravelAgentId { get; set; }
        public virtual TravelAgent TravelAgent { get; set; } = null!;
        public string? AssignedToId { get; set; }
        public virtual ApplicationUser? AssignedTo { get; set; }
        public virtual Quotation? Quotation { get; set; }
        public virtual Booking? Booking { get; set; }
    }
}
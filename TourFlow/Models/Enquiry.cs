using TourFlow.Data;
using TourFlow.Client.Enums;
using TourFlow.Client.Models;

namespace TourFlow.Models
{
    public class Enquiry
    {
        public int Id { get; set; }
        public string? TravelAgentName { get; set; }
        public int GroupSize { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string? Destination { get; set; }
        public string? Budget { get; set; }
        public string? RequestedServices { get; set; }
        public string? AdditionalNotes { get; set; }
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? Updated { get; set; }
        public EnquiryStatus Status { get; set; } = EnquiryStatus.New;
        public string? CreatedById { get; set; }

        // Navigation properties
        public string? AssignedToId { get; set; }
        public virtual ApplicationUser? AssignedTo { get; set; }
        public int? BookingId { get; set; }
        public virtual Booking? Booking { get; set; }
    }

    public static class EnquiryExtensions
    {
        public static EnquiryDTO ToDTO(this Enquiry e)
        {
            return new EnquiryDTO
            {
                Id = e.Id,
                GroupSize = e.GroupSize,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Destination = e.Destination ?? string.Empty,
                Budget = e.Budget ?? string.Empty,
                RequestedServices = e.RequestedServices ?? string.Empty,
                AdditionalNotes = e.AdditionalNotes ?? string.Empty,
                Created = e.Created,
                Updated = e.Updated,
                Status = e.Status,
                AssignedTo = e.AssignedTo?.ToDTO(),
                Booking = e.Booking?.ToDTO()
            };
        }
    }
}
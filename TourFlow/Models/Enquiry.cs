using TourFlow.Data;
using TourFlow.Client.Enums;
using TourFlow.Client.Models;

namespace TourFlow.Models
{
    public class Enquiry
    {
        // Fields
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        private DateTimeOffset _startDate;
        private DateTimeOffset _endDate;

        public int Id { get; set; }
        public int GroupSize { get; set; }
        public DateTimeOffset StartDate
        {
            get => _startDate;
            set => _startDate = value.ToUniversalTime();
        }

        public DateTimeOffset EndDate
        {
            get => _endDate;
            set => _endDate = value.ToUniversalTime();
        }
        public string? Destination { get; set; }
        public string? Budget { get; set; }
        public string? RequestedServices { get; set; }
        public string? AdditionalNotes { get; set; }
        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }
        public DateTimeOffset? Updated
        {
            get => _updated;
            set => _updated = value?.ToUniversalTime();
        }
        public EnquiryStatus Status { get; set; } = EnquiryStatus.New;

        // Navigation properties
        public int TravelAgentId { get; set; }
        public virtual TravelAgent TravelAgent { get; set; } = null!;
        public string? AssignedToId { get; set; }
        public virtual ApplicationUser? AssignedTo { get; set; }
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
                Booking = e.Booking?.ToDTO(),
                AssignedTo = e.AssignedTo?.ToDTO()
            };
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class EnquiryDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public int Id { get; set; }
        public int GroupSize { get; set; }
        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => _startDate = value?.ToUniversalTime();
        }

        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => _endDate = value?.ToUniversalTime();
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
        public EnquiryStatus Status { get; set; }
        public BookingDTO? Booking { get; set; }
        public UserDTO? AssignedTo { get; set; }

        #region Helper properties

        [Required, JsonIgnore]
        public DateTime? StartDateTime
        {
            get => StartDate?.DateTime;
            set => StartDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        [Required, JsonIgnore]
        public DateTime? EndDateTime
        {
            get => EndDate?.DateTime;
            set => EndDate = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        #endregion
    }
}
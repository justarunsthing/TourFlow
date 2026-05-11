using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class EnquiryDTO : IValidatableObject
    {
        // Fields
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        private DateTimeOffset? _startDate;
        private DateTimeOffset? _endDate;

        public int Id { get; set; }
        public string? TravelAgentName { get; set; }

        [Required]
        [Range(14, int.MaxValue, ErrorMessage = "Group size must be at least 14 people")]
        [Description("The number of people in the tour")]
        public int GroupSize { get; set; }

        // From auto property to full property
        [Description("The date and time when the enquiry was created, stored in UTC")]
        public DateTimeOffset Created
        {
            get => _created;
            set => _created = value.ToUniversalTime();
        }

        [Description("The date and time when the enquiry was updated, stored in UTC")]
        public DateTimeOffset? Updated
        {
            get => _updated;
            set => _updated = value?.ToUniversalTime();
        }

        [Required(ErrorMessage = "Start date is required")]
        [Description("The date and time when the tour is scheduled to start, stored in UTC")]
        public DateTimeOffset? StartDate
        {
            get => _startDate;
            set => _startDate = value?.ToUniversalTime();
        }

        [Required(ErrorMessage = "End date is required")]
        [Description("The date and time when the tour is scheduled to end, stored in UTC")]
        public DateTimeOffset? EndDate
        {
            get => _endDate;
            set => _endDate = value?.ToUniversalTime();
        }

        [Required]
        [Description("The destination of the tour")]
        public string Destination { get; set; } = string.Empty;

        [Required]
        [Description("The budget range for the tour")]
        public string Budget { get; set; } = string.Empty;

        [Description("The requested services for the tour")]
        public string? RequestedServices { get; set; }

        [Description("Any additional notes for the tour")]
        public string? AdditionalNotes { get; set; }
        public EnquiryStatus Status { get; set; }
        public UserDTO? AssignedTo { get; set; }
        public BookingDTO? Booking { get; set; }

        #region Helper properties

        [Required, JsonIgnore]
        public DateTime? StartDateTime
        {
            get => StartDate?.UtcDateTime;
            set => StartDate = value.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc))
                : null;
        }

        [Required, JsonIgnore]
        public DateTime? EndDateTime
        {
            get => EndDate?.UtcDateTime;
            set => EndDate = value.HasValue
                ? new DateTimeOffset(DateTime.SpecifyKind(value.Value, DateTimeKind.Utc))
                : null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Start date must be in the future (after today)
            if (StartDate.HasValue)
            {
                var today = DateTimeOffset.UtcNow.Date; // Use .Date to ignore time

                if (StartDate.Value.Date <= today)
                {
                    results.Add(new ValidationResult("Start date must be in the future (cannot be today or in the past).", [nameof(StartDate)]));
                }
            }

            // End date must be after start date
            if (StartDate.HasValue && EndDate.HasValue)
            {
                if (EndDate.Value.Date < StartDate.Value.Date)
                {
                    results.Add(new ValidationResult("End date cannot be before the start date.", [nameof(EndDate)]));
                }
            }

            return results;
        }

        #endregion
    }
}
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class EnquiryDTO
    {
        public int Id { get; set; }
        public int GroupSize { get; set; }

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }

        public string Destination { get; set; } = string.Empty;
        public string Budget { get; set; } = string.Empty;
        public string? RequestedServices { get; set; }
        public string? AdditionalNotes { get; set; }

        public EnquiryStatus Status { get; set; }

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }

        public UserDTO? AssignedTo { get; set; }
        public BookingDTO? Booking { get; set; }
    }
}
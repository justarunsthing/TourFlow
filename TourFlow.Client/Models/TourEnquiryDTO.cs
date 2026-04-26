using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class TourEnquiryDTO
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
        public EnquiryStatus Status { get; set; }
        public int TravelAgentId { get; set; }
        public string? TravelAgentCompanyName { get; set; }
        public string? AssignedToId { get; set; }
        public string? AssignedToFullName { get; set; }
        public int? QuotationId { get; set; }
        public string? QuotationNumber { get; set; }
        public int? BookingId { get; set; }
        public string? BookingNumber { get; set; }
    }
}
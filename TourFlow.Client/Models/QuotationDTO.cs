using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class QuotationDTO
    {
        public int Id { get; set; }
        public string? QuotationNumber { get; set; }
        public int TourEnquiryId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "GBP";
        public string? AIItinerarySummary { get; set; }
        public string? AdditionalNotes { get; set; }
        public QuotationStatus Status { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? SentAt { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string? EnquiryNumber { get; set; }
        public int? BookingId { get; set; }
    }
}
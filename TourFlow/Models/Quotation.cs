using TourFlow.Client.Enums;

namespace TourFlow.Models
{
    public class Quotation
    {
        public int Id { get; set; }
        public string? QuotationNumber { get; set; }
        public int TourEnquiryId { get; set; }
        public virtual TourEnquiry TourEnquiry { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "GBP";
        public string? AIItinerarySummary { get; set; }
        public string? AdditionalNotes { get; set; }
        public QuotationStatus Status { get; set; } = QuotationStatus.Draft;
        public DateTimeOffset Created { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? SentAt { get; set; }
        public DateTimeOffset? Updated { get; set; }

        // Navigation properties
        public virtual Booking? Booking { get; set; }
    }
}
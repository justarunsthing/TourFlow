using TourFlow.Client.Enums;

namespace TourFlow.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string? BookingNumber { get; set; }
        public int TourEnquiryId { get; set; }
        public virtual TourEnquiry TourEnquiry { get; set; } = null!;
        public int? QuotationId { get; set; }
        public virtual Quotation? Quotation { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "GBP";
        public BookingStatus Status { get; set; } = BookingStatus.New;
        public DateTimeOffset BookingDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? ConfirmedAt { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
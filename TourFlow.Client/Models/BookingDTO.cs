using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public string? BookingNumber { get; set; }
        public int TourEnquiryId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "GBP";
        public BookingStatus Status { get; set; }
        public DateTimeOffset BookingDate { get; set; }
        public DateTimeOffset? ConfirmedAt { get; set; }
        public string? EnquiryNumber { get; set; }
        public string? QuotationNumber { get; set; }
    }
}
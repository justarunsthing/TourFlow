using TourFlow.Client.Enums;
using TourFlow.Client.Models;

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

    public static class BookingExtensions
    {
        public static BookingDTO ToDTO(this Booking b)
        {
            return new BookingDTO
            {
                Id = b.Id,
                BookingNumber = b.BookingNumber ?? string.Empty,
                TourEnquiryId = b.TourEnquiryId,
                TotalAmount = b.TotalAmount,
                Currency = b.Currency,
                Status = b.Status,
                BookingDate = b.BookingDate,
                ConfirmedAt = b.ConfirmedAt,
                EnquiryNumber = b.TourEnquiry?.EnquiryNumber,
                QuotationNumber = b.Quotation?.QuotationNumber
            };
        }
    }
}
using TourFlow.Client.Enums;
using TourFlow.Client.Models;

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

    public static class QuotationExtensions
    {
        public static QuotationDTO ToDTO(this Quotation q)
        {
            return new QuotationDTO
            {
                Id = q.Id,
                QuotationNumber = q.QuotationNumber ?? string.Empty,
                TourEnquiryId = q.TourEnquiryId,
                TotalAmount = q.TotalAmount,
                Currency = q.Currency,
                AIItinerarySummary = q.AIItinerarySummary,
                AdditionalNotes = q.AdditionalNotes,
                Status = q.Status,
                Created = q.Created,
                SentAt = q.SentAt,
                Updated = q.Updated,
                EnquiryNumber = q.TourEnquiry?.EnquiryNumber
            };
        }
    }
}
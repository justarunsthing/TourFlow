using System.ComponentModel.DataAnnotations;
using TourFlow.Client.Enums;
using TourFlow.Client.Models;
using TourFlow.Data;

namespace TourFlow.Models
{
    public class Booking
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "GBP";
        public BookingStatus Status { get; set; }
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
        public TourProvider TourProvider { get; set; }

        [Required]
        public int EnquiryId { get; set; }
        public virtual Enquiry Enquiry { get; set; } = null!;
        public string? CreatedById { get; set; }
        public virtual ApplicationUser? CreatedBy { get; set; }
        public virtual BookingAttachment? Attachment { get; set; }
    }

    public static class BookingExtensions
    {
        public static BookingDTO ToDTO(this Booking b)
        {
            return new BookingDTO
            {
                Id = b.Id,
                Description = b.Description,
                TotalAmount = b.TotalAmount,
                Currency = b.Currency,
                Status = b.Status,
                Created = b.Created,
                Updated = b.Updated,
                EnquiryId = b.EnquiryId,
                CreatedById = b.CreatedById,
                CreatedBy = b.CreatedBy?.ToDTO(),
                TourProvider = b.TourProvider
            };
        }
    }
}
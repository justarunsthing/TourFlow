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
        public BookingStatus Status { get; set; } = BookingStatus.InProgress;
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
        public int EnquiryId { get; set; }
        public virtual Enquiry? Enquiry { get; set; }
        public string? CreatorUserId { get; set; }
        public virtual ApplicationUser? CreatorUser { get; set; }
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
                Enquiry = b.Enquiry?.ToDTO(),
                CreatorUserId = b.CreatorUserId,
                CreatorUser = b.CreatorUser?.ToDTO()
            };
        }
    }
}
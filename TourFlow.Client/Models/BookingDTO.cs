using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class BookingDTO
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
        public int EnquiryId { get; set; }
        public virtual EnquiryDTO? Enquiry { get; set; }
        public string? CreatorUserId { get; set; }
        public virtual UserDTO? CreatorUser { get; set; }
    }
}
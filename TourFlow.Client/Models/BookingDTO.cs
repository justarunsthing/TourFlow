using System.ComponentModel.DataAnnotations;
using TourFlow.Client.Enums;

namespace TourFlow.Client.Models
{
    public class BookingDTO
    {
        private DateTimeOffset _created;
        private DateTimeOffset? _updated;
        public int Id { get; set; }
        public int EnquiryId { get; set; }

        [Required(ErrorMessage = "Please provide a detailed description")]
        public string? Description { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; } = "GBP";

        [Required]
        public TourProvider TourProvider { get; set; }
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
        public string? CreatedById { get; set; }
        public UserDTO? CreatedBy { get; set; }
        public AttachmentDTO? Attachment { get; set; }
    }
}
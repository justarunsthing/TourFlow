using TourFlow.Client.Models;

namespace TourFlow.Models
{
    public class BookingAttachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; } = null!;
        public Guid FileUploadId { get; set; }
        public virtual FileUpload FileUpload { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;
    }

    public static class BookingAttachmentExtensions
    {
        public static AttachmentDTO ToDTO(this BookingAttachment a)
        {
            return new AttachmentDTO
            {
                Id = a.Id,
                FileName = a.FileName,
                Url = $"/uploads/{a.FileUploadId}"
            };
        }
    }
}
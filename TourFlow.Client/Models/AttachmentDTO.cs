namespace TourFlow.Client.Models
{
    public class AttachmentDTO
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
namespace TourFlow.Client.Models
{
    public class TravelAgentDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string ImageUrl { get; set; } = $"https://api.dicebear.com/9.x/glass/svg?seed={Random.Shared.Next()}";
        public ICollection<EnquiryDTO> Enquiries { get; set; } = [];
        public ICollection<UserDTO> Members { get; set; } = [];

    }
}
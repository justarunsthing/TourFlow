using TourFlow.Data;
using TourFlow.Client.Models;
using System.ComponentModel.DataAnnotations;

namespace TourFlow.Models
{
    public class TravelAgent
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid? ImageId { get; set; } // FK

        // Navigation properties
        public virtual FileUpload? Image { get; set; }
        public virtual ICollection<Enquiry> Enquiries { get; set; } = [];
        public virtual ICollection<ApplicationUser> Members { get; set; } = [];
    }

    public static class TravelAgentExtensions
    {
        public static TravelAgentDTO ToDTO(this TravelAgent agent)
        {
            return new TravelAgentDTO
            {
                Id = agent.Id,
                Name = agent.Name,
                ImageUrl = agent.ImageId.HasValue
                    ? $"uploads/{agent.ImageId}"
                    : $"https://api.dicebear.com/9.x/glass/svg?seed={agent.Name}",
                Enquiries = [..agent.Enquiries.Select(e => e.ToDTO())],
                Members = [..agent.Members.Select(e => e.ToDTO())]
            };
        }
    }
}
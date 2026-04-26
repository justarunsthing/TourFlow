using TourFlow.Client.Models;

namespace TourFlow.Models
{
    public class TravelAgent
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }
        public string Email { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<TourEnquiry> TourEnquiries { get; set; } = [];
    }

    public static class TravelAgentExtensions
    {
        public static TravelAgentDTO ToDTO(this TravelAgent agent)
        {
            return new TravelAgentDTO
            {
                Id = agent.Id,
                CompanyName = agent.CompanyName ?? string.Empty,
                ContactPerson = agent.ContactPerson ?? string.Empty,
                Phone = agent.Phone ?? string.Empty,
                Email = agent.Email
            };
        }
    }
}
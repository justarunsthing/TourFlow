namespace TourFlow.Models
{
    public class TravelAgent
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Phone { get; set; }

        // Navigation properties
        public ICollection<TourEnquiry> TourEnquiries { get; set; } = [];
    }
}
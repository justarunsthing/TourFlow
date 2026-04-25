using TourFlow.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TourFlow.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public Guid? ProfilePictureId { get; set; }
        public virtual ImageUpload? ProfilePicture { get; set; }

        // Navigation properties
        public int TravelAgentId { get; set; }
        public virtual TravelAgent? TravelAgent { get; set; }
        public virtual ICollection<TourEnquiry> Enquiries { get; set; } = [];
    }
}

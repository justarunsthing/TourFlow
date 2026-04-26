using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TourFlow.Client.Models;
using TourFlow.Models;

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
        public virtual ICollection<TourEnquiry> AssignedEnquiries { get; set; } = [];
    }

    public static class ApplicationUserExtensions
    {
        public static UserDTO ToDTO(this ApplicationUser user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                ImageUrl = user.ProfilePictureId.HasValue
                    ? $"uploads/{user.ProfilePictureId}"
                    : $"https://api.dicebear.com/9.x/glass/svg?seed={user.FirstName}{user.LastName}"
            };
        }
    }
}
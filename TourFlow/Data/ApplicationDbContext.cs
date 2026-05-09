using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourFlow.Models;

namespace TourFlow.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ImageUpload> Images { get; set; }
        public DbSet<FileUpload> Uploads { get; set; }
        public DbSet<TravelAgent> TravelAgents { get; set; }
        public DbSet<Enquiry> TourEnquiries { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
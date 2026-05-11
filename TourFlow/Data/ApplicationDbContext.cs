using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourFlow.Models;

namespace TourFlow.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<ImageUpload> Images { get; set; }
        public DbSet<FileUpload> Uploads { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<TravelAgent> TravelAgents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Enquiry)
                .WithOne(e => e.Booking)
                .HasForeignKey<Booking>(b => b.EnquiryId) // Booking is the dependent side
                .OnDelete(DeleteBehavior.Cascade); // Delete booking if enquiry is deleted
        }
    }
}
using TourFlow.Data;
using TourFlow.Models;
using TourFlow.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TourFlow.Repository
{
    public class EnquiryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IEnquiryRepository
    {
        public async Task<IEnumerable<Enquiry>> GetAllEnquiriesAsync()
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            return await context.TourEnquiries
                .Include(e => e.TravelAgent)
                .Include(e => e.AssignedTo)
                .Include(e => e.Booking)
                .ToListAsync();
        }
    }
}
using TourFlow.Data;
using TourFlow.Models;
using TourFlow.Interfaces;
using Microsoft.EntityFrameworkCore;
using TourFlow.Client;

namespace TourFlow.Repository
{
    public class EnquiryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IEnquiryRepository
    {
        public async Task<IEnumerable<Enquiry>> GetAllEnquiriesAsync()
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            return await context.Enquiries
                .Include(e => e.AssignedTo)
                .ToListAsync();
        }

        public async Task<Enquiry> CreateEnquiryAsync(Enquiry enquiry, UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            enquiry.CreatedById = user.UserId;
            enquiry.TravelAgentId = user.TravelAgentId;
            enquiry.Created = DateTimeOffset.UtcNow;

            context.Add(enquiry);
            await context.SaveChangesAsync();

            return enquiry;
        }
    }
}
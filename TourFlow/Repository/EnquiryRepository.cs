using TourFlow.Data;
using TourFlow.Models;
using TourFlow.Interfaces;
using Microsoft.EntityFrameworkCore;
using TourFlow.Client;
using TourFlow.Client.Enums;

namespace TourFlow.Repository
{
    public class EnquiryRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IEnquiryRepository
    {
        public async Task<Enquiry?> GetEnquiryByIdAsync(int id)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Enquiry? enquiry = await context.Enquiries
                .FirstOrDefaultAsync(e  => e.Id == id);

            return enquiry;
        }

        public async Task<IEnumerable<Enquiry>> GetAllEnquiriesAsync(UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            if (user.TravelAgentId != 0)
            {
                return await context.Enquiries
                    .Where(e => e.TravelAgentId == user.TravelAgentId)
                    .Include(e => e.Booking)
                    .ToListAsync();
            }
            else
            {
                return await context.Enquiries
                    .Include(e => e.Booking)
                    .ToListAsync();
            }
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

        public async Task UpdateEnquiryAsync(int enquiryId, UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Enquiry enquiry = await context.Enquiries.FirstAsync(e => e.Id == enquiryId);

            enquiry.Status = EnquiryStatus.Quoted;
            enquiry.Updated = DateTimeOffset.UtcNow;

            await context.SaveChangesAsync();
        }
    }
}
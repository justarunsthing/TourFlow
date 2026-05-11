using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using TourFlow.Client;
using TourFlow.Client.Enums;
using TourFlow.Data;
using TourFlow.Interfaces;
using TourFlow.Models;

namespace TourFlow.Repository
{
    public class BookingRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IBookingRepository
    {
        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Booking? booking = await context.Bookings
                .Include(b => b.Enquiry)
                .Include(b => b.CreatedBy)
                .Include(b => b.Attachment)
                    .ThenInclude(a => a.FileUpload)
                .FirstOrDefaultAsync(b => b.Id == bookingId);

            return booking;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync(UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            if (user.TravelAgentId != 0)
            {
                return await context.Bookings
                    .Where(b => b.Enquiry.TravelAgentId == user.TravelAgentId)
                    .Include(b => b.Enquiry)
                    .Include(b => b.CreatedBy)
                    .Include(b => b.Attachment)
                        .ThenInclude(a => a.FileUpload)
                    .ToListAsync();
            }
            else
            {
                return await context.Bookings
                    .Include(b => b.Enquiry)
                    .Include(b => b.CreatedBy)
                    .Include(b => b.Attachment)
                        .ThenInclude(a => a.FileUpload)
                    .ToListAsync();
            }
        }

        public async Task<Booking> CreateBookingAsync(Booking booking, UserInfo user, IBrowserFile file)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            booking.Created = DateTimeOffset.UtcNow;
            booking.CreatedById = user.UserId;
            booking.Status = BookingStatus.InProgress;

            context.Bookings.Add(booking);
            await context.SaveChangesAsync();

            if (file != null)
            {
                try
                {
                    await using var stream = file.OpenReadStream(maxAllowedSize: 15 * 1024 * 1024);

                    using var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);

                    var fileBytes = memoryStream.ToArray();
                    var fileUpload = new FileUpload
                    {
                        Id = Guid.NewGuid(),
                        Data = fileBytes,
                        Type = file.ContentType
                    };

                    var attachment = new BookingAttachment
                    {
                        BookingId = booking.Id,
                        FileUploadId = fileUpload.Id,
                        FileName = file.Name,
                        ContentType = file.ContentType,
                        FileSize = file.Size,
                        UploadedAt = DateTimeOffset.UtcNow
                    };

                    context.Uploads.Add(fileUpload);
                    context.BookingAttachments.Add(attachment);

                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return booking;
        }

        public async Task CancelBookingAsync(int bookingId, UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Booking? booking = await context.Bookings
                .Include(b => b.Enquiry)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.Enquiry.TravelAgentId == user.TravelAgentId);

            if (booking == null)
            {
                return;
            }

            booking.Status = BookingStatus.Cancelled;
            booking.Updated = DateTimeOffset.UtcNow;
            booking.Enquiry.Status = EnquiryStatus.Rejected;

            await context.SaveChangesAsync();
        }

        public async Task ConfirmBookingAsync(int bookingId, UserInfo user)
        {
            await using ApplicationDbContext context = contextFactory.CreateDbContext();

            Booking? booking = await context.Bookings
                .Include(b => b.Enquiry)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.Enquiry.TravelAgentId == user.TravelAgentId);

            if (booking == null)
            {
                return;
            }

            booking.Status = BookingStatus.Confirmed;
            booking.Updated = DateTimeOffset.UtcNow;
            booking.Enquiry.Status = EnquiryStatus.Converted;

            await context.SaveChangesAsync();
        }
    }
}
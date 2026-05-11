using Microsoft.AspNetCore.Components.Forms;
using TourFlow.Client;
using TourFlow.Client.Enums;
using TourFlow.Client.Interfaces;
using TourFlow.Client.Models;
using TourFlow.Data;
using TourFlow.Helpers;
using TourFlow.Models;

namespace TourFlow.Services
{
    public class BookingDTOService(ApplicationDbContext context) : IBookingDTOService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<BookingDTO> CreateBookingAsync(BookingDTO dto, UserInfo user, IBrowserFile file)
        {
            var booking = new Booking
            {
                EnquiryId = dto.EnquiryId,
                Description = dto.Description,
                TotalAmount = dto.TotalAmount,
                Currency = dto.Currency,
                TourProvider = dto.TourProvider,
                Status = BookingStatus.InProgress,
                CreatedById = user.UserId,
                Created = DateTimeOffset.UtcNow
            };

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();

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

                    _context.Uploads.Add(fileUpload);
                    _context.BookingAttachments.Add(attachment);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("******** EMAIL SERVICE ********");
            Console.WriteLine($"You have received a new quotation from Tour Flow");
            Console.WriteLine($"A new enquiry email has been sent to travel agent");
            Console.WriteLine("******** EMAIL SERVICE ********");

            return booking.ToDTO();
        }
    }
}
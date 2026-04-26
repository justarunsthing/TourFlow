using TourFlow.Data;
using TourFlow.Client.Enums;
using TourFlow.Client.Models;

namespace TourFlow.Models
{
    public class TourEnquiry
    {
        public int Id { get; set; }
        public string? EnquiryNumber { get; set; } 
        public int GroupSize { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string? Destination { get; set; }
        public string? Budget { get; set; }
        public string? RequestedServices { get; set; }
        public string? AdditionalNotes { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public EnquiryStatus Status { get; set; } = EnquiryStatus.New;

        // Navigation properties
        public int TravelAgentId { get; set; }
        public virtual TravelAgent TravelAgent { get; set; } = null!;
        public string? AssignedToId { get; set; }
        public virtual ApplicationUser? AssignedTo { get; set; }
        public virtual Quotation? Quotation { get; set; }
        public virtual Booking? Booking { get; set; }
    }

    public static class TourEnquiryExtensions
    {
        public static TourEnquiryDTO ToDTO(this TourEnquiry e)
        {
            return new TourEnquiryDTO
            {
                Id = e.Id,
                EnquiryNumber = e.EnquiryNumber ?? string.Empty,
                GroupSize = e.GroupSize,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                Destination = e.Destination ?? string.Empty,
                Budget = e.Budget ?? string.Empty,
                RequestedServices = e.RequestedServices ?? string.Empty,
                AdditionalNotes = e.AdditionalNotes ?? string.Empty,
                Created = e.Created,
                Updated = e.Updated,
                Status = e.Status,
                TravelAgentId = e.TravelAgentId,
                TravelAgentCompanyName = e.TravelAgent?.CompanyName ?? string.Empty,
                AssignedToId = e.AssignedToId,
                AssignedToFullName = e.AssignedTo != null ? $"{e.AssignedTo.FirstName} {e.AssignedTo.LastName}" : null,
                QuotationId = e.Quotation?.Id,
                QuotationNumber = e.Quotation?.QuotationNumber,
                BookingId = e.Booking?.Id,
                BookingNumber = e.Booking?.BookingNumber
            };
        }
    }
}
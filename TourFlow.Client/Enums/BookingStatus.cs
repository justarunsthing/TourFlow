using System.ComponentModel.DataAnnotations;

namespace TourFlow.Client.Enums
{
    public enum BookingStatus
    {
        [Display(Name = "In Progress")]
        InProgress,
        Confirmed,
        Cancelled
    }
}
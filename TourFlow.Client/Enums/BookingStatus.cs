using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TourFlow.Client.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BookingStatus
    {
        [Display(Name = "In Progress")]
        InProgress,
        Confirmed,
        Cancelled
    }
}
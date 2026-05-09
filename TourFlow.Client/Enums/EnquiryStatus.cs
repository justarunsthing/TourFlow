using System.Text.Json.Serialization;

namespace TourFlow.Client.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EnquiryStatus
    {
        New,
        Quoted,
        Converted,
        Rejected
    }
}
using System.Text.Json.Serialization;

namespace TourFlow.Client.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        Admin,
        Manager,
        Sales,
        TravelAgent
    }
}
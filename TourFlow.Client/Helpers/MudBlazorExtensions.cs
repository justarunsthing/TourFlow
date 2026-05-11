using MudBlazor;
using TourFlow.Client.Enums;

namespace TourFlow.Client.Helpers
{
    public static class MudBlazorExtensions
    {
        public static Color GetColor(this EnquiryStatus priority)
        {
            Color color = priority switch
            {
                EnquiryStatus.New => Color.Info,
                EnquiryStatus.Quoted => Color.Secondary,
                EnquiryStatus.Converted => Color.Success,
                EnquiryStatus.Rejected => Color.Error,
                _ => Color.Default
            };

            return color;
        }

        public static Color GetColor(this BookingStatus priority)
        {
            Color color = priority switch
            {
                BookingStatus.InProgress => Color.Info,
                BookingStatus.Confirmed => Color.Success,
                BookingStatus.Cancelled => Color.Error,
                _ => Color.Default
            };

            return color;
        }
    }
}
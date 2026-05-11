using System.ComponentModel.DataAnnotations;

namespace TourFlow.Client.Enums
{
    public enum TourProvider
    {
        [Display(Name = "Select Provider")]
        None = 0,

        [Display(Name = "Premium Tours Ltd")]
        PremiumTours,

        [Display(Name = "Elite Travel Group")]
        EliteTravel,

        [Display(Name = "Global Explorer")]
        GlobalExplorer,

        [Display(Name = "Vista Holidays")]
        VistaHolidays,

        [Display(Name = "Heritage Journeys")]
        HeritageJourneys,

        [Display(Name = "Luxury Escapes")]
        LuxuryEscapes
    }
}
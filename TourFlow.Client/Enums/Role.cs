using System.ComponentModel.DataAnnotations;

namespace TourFlow.Client.Enums
{
    public enum Role
    {
        Admin,

        [Display(Name = "Operations Manager")]
        OperationsManager,

        [Display(Name = "Sales Executive")]
        SalesExecutive,
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.ViewModels
{
    public class BodyDataViewModel
    {

        [Display(Name = "Last Reviewed")]
        public DateTime LastReviewed { get; set; }
    }
}

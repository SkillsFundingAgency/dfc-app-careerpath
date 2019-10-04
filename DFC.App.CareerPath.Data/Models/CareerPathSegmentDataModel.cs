using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentDataModel
    {
        [Required]
        public DateTime? LastReviewed { get; set; }

        [Required]
        public string Markup { get; set; }
    }
}

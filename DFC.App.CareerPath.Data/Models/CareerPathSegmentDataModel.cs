using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentDataModel
    {
        public string SegmentName { get; private set; } = "CareerPath";

        [Required]
        public DateTime? LastReviewed { get; set; }

        [Required]
        public string Markup { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentDataModel
    {
        public const string SegmentName = "CareerPathsAndProgression";

        [Required]
        public string Markup { get; set; }
    }
}

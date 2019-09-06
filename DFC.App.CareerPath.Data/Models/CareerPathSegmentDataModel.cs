using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentDataModel
    {
        [Display(Name = "Last Reviewed")]
//        [JsonProperty(PropertyName = "lastReviewed")]
        public DateTime LastReviewed { get; set; }
    }
}

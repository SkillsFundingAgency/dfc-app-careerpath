using DFC.App.CareerPath.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentModel : IDataModel
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [Required]
   //     [JsonProperty(PropertyName = "canonicalName")]
        public string CanonicalName { get; set; }

        public string Markup { get; set; }

     //   [JsonProperty(PropertyName = "data")]
        public CareerPathSegmentDataModel Data { get; set; }
    }
}

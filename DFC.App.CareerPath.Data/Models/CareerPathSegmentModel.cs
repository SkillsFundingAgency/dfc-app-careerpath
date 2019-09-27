using DFC.App.CareerPath.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentModel : IDataModel
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        [Required]
        public string SocCodeTwo { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        public string PartitionKey => SocCodeTwo?.Substring(0, 2);

        public CareerPathSegmentDataModel Data { get; set; }
    }
}

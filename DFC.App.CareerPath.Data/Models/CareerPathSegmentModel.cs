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
        public string CanonicalName { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public int PartitionKey => Created.Second;

        public DateTime Updated { get; set; }

        public string Markup { get; set; }

        public CareerPathSegmentDataModel Data { get; set; }
    }
}

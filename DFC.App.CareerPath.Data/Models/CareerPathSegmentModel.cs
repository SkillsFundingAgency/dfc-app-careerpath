﻿using DFC.App.CareerPath.Data.Contracts;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models
{
    public class CareerPathSegmentModel : IDataModel
    {
        public const string SegmentName = "CareerPath";

        [Required]
        [JsonProperty(PropertyName = "id")]
        public Guid DocumentId { get; set; }

        [JsonProperty(PropertyName = "_etag")]
        public string Etag { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        [Required]
        public string SocLevelTwo { get; set; }

        public string PartitionKey => SocLevelTwo;

        [Required]
        public DateTime? LastReviewed { get; set; }

        public CareerPathSegmentDataModel Data { get; set; }
    }
}

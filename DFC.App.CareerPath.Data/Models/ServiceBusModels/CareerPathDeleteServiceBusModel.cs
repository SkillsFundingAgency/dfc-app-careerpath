using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models.ServiceBusModels
{
    public class CareerPathDeleteServiceBusModel
    {
        [Required]
        public Guid JobProfileId { get; set; }

        [Required]
        public DateTime LastReviewed { get; set; }
    }
}

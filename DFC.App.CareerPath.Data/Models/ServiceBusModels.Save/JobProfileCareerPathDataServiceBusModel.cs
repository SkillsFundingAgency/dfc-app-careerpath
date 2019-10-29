using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models.ServiceBusModels.Save
{
    public class JobProfileCareerPathDataServiceBusModel
    {
        [Required]
        public DateTime? LastReviewed { get; set; }

        [Required]
        public string Markup { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models.ServiceBusModels
{
    public class CareerPathPatchMarkupServiceBusModel
    {
        [Required]
        public Guid JobProfileId { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        [Required]
        public string SocLevelTwo { get; set; }

        [Required]
        public CareerPathSegmentDataModel Data { get; set; }
    }
}

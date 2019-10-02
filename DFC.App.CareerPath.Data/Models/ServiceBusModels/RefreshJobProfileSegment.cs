using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models.ServiceBusModels
{
    public class RefreshJobProfileSegment
    {
        [Required]
        public Guid JobProfileId { get; set; }

        [Required]
        public string Segment { get; set; }
    }
}

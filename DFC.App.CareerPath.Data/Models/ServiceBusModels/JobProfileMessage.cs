using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models.ServiceBusModels
{
    public class JobProfileMessage
    {
        public Guid JobProfileId { get; set; }

        [Required]
        public string SOCLevelTwo { get; set; }

        public DateTime LastModified { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        public long SequenceNumber { get; set; }

        public string CareerPathAndProgression { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.CareerPath.Data.Models.ServiceBusModels.Save
{
    public class JobProfileServiceBusSaveModel
    {
        public Guid JobProfileId { get; set; }

        [Required]
        public string SOCLevelTwo { get; set; }

        [Required]
        public string CanonicalName { get; set; }

        public long SequenceNumber { get; set; }

        public string CareerPathAndProgression { get; set; }
    }
}

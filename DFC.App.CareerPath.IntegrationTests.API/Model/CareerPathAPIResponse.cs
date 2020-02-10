using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model
{
    internal class CareerPathAPIResponse
    {
        [JsonProperty("careerPathAndProgression")]
        public List<string> CareerPathAndProgression { get; set; }
    }
}

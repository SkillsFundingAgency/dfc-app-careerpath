using Newtonsoft.Json;
using System.Collections.Generic;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.API
{
    public class CareerPathAPIResponseBody
    {
        [JsonProperty("careerPathAndProgression")]
        public List<string> CareerPathAndProgression { get; set; }
    }
}

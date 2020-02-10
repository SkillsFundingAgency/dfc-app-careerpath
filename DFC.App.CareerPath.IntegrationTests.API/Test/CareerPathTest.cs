using DFC.App.RelatedCareers.Tests.Common.APISupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Test
{
    public class CareerPathTest : SetUpAndTearDown
    {
        [Test]
        [Description("Tests that the CType 'JobProfile' successfully tiggers a related career path update to an existing job profile")]
        public async Task CareerPathJobProfile()
        {
            Response<CareerPathAPIResponse> response = await this.CommonAction.ExecuteGetRequest<CareerPathAPIResponse>(Settings.APIConfig.EndpointBaseUrl.Replace("{id}", this.JobProfile.JobProfileId, System.StringComparison.CurrentCultureIgnoreCase), GetRequest.ContentType.Json).ConfigureAwait(true);
            Assert.AreEqual(this.JobProfile.CareerPathAndProgression, response.Data.CareerPathAndProgression[0]);
        }
    }
}
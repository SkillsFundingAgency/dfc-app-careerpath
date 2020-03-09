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
            var response = await this.API.GetById(this.JobProfile.JobProfileId).ConfigureAwait(true);
            Assert.AreEqual(this.JobProfile.CareerPathAndProgression, response.Data.CareerPathAndProgression[0]);
        }
    }
}
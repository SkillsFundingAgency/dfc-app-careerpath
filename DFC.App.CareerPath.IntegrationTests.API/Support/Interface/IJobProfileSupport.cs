using DFC.App.CareerPath.Tests.Common.AzureServiceBusSupport;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.ContentType.JobProfile;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    internal interface IJobProfileSupport
    {
        JobProfileContentType GenerateJobProfileContentType();

        Task DeleteJobProfile(Topic topic, JobProfileContentType jobProfileId);
    }
}

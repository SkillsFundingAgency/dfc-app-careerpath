using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model;
using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.JobProfile;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.Interface
{
    internal interface IRelatedCareersSupport
    {
        RelatedCareersData GenerateRelatedCareersDataSection();
    }
}

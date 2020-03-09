using DFC.App.RelatedCareers.Tests.IntegrationTests.API.Model.API;
using RestSharp;
using System.Threading.Tasks;

namespace DFC.App.RelatedCareers.Tests.IntegrationTests.API.Support.API
{
    public interface ICareerPathAPI
    {
        Task<IRestResponse<CareerPathAPIResponseBody>> GetById(string id);
    }
}

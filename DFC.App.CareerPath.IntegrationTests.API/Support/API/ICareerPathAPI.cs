using DFC.App.CareerPath.Tests.IntegrationTests.API.Model.API;
using RestSharp;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Tests.IntegrationTests.API.Support.API
{
    public interface ICareerPathAPI
    {
        Task<IRestResponse<CareerPathAPIResponse>> GetById(string id);
    }
}

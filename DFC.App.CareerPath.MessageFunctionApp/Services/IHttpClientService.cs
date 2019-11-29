using DFC.App.CareerPath.Data.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.MessageFunctionApp.Services
{
    public interface IHttpClientService
    {
        Task<HttpStatusCode> PostAsync(CareerPathSegmentModel careerPathSegmentModel);

        Task<HttpStatusCode> PutAsync(CareerPathSegmentModel careerPathSegmentModel);

        Task<HttpStatusCode> DeleteAsync(Guid id);
    }
}
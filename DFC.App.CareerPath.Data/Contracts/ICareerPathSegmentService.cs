using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface ICareerPathSegmentService
    {
        Task<bool> PingAsync();

        Task<IEnumerable<CareerPathSegmentModel>> GetAllAsync();

        Task<CareerPathSegmentModel> GetByIdAsync(Guid documentId);

        Task<CareerPathSegmentModel> GetByNameAsync(string canonicalName);

        Task<HttpStatusCode> UpsertAsync(CareerPathSegmentModel careerPathSegmentModel);

        Task<bool> DeleteAsync(Guid documentId);
    }
}

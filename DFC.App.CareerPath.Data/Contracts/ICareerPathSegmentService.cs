using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Data.Contracts
{
    public interface ICareerPathSegmentService
    {
        Task<IEnumerable<CareerPathSegmentModel>> GetAllAsync();

        Task<CareerPathSegmentModel> GetByIdAsync(Guid documentId);

        Task<CareerPathSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false);
    }
}

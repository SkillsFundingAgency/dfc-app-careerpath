using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.SegmentService
{
    public class CareerPathSegmentService : ICareerPathSegmentService
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly ICareerPathDraftSegmentService careerPathDraftSegmentService;

        public CareerPathSegmentService(ICosmosRepository<CareerPathSegmentModel> repository, ICareerPathDraftSegmentService careerPathDraftSegmentService)
        {
            this.repository = repository;
            this.careerPathDraftSegmentService = careerPathDraftSegmentService;
        }

        public async Task<IEnumerable<CareerPathSegmentModel>> GetAllAsync()
        {
            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<CareerPathSegmentModel> GetByIdAsync(Guid documentId)
        {
            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<CareerPathSegmentModel> GetByNameAsync(string canonicalName, bool isDraft = false)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return isDraft
                ? await careerPathDraftSegmentService.GetSitefinityData(canonicalName.ToLowerInvariant()).ConfigureAwait(false)
                : await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }
    }
}

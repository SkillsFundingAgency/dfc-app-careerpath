using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.SegmentService
{
    public class CareerPathSegmentService : ICareerPathSegmentService
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;

        public CareerPathSegmentService(ICosmosRepository<CareerPathSegmentModel> repository, IDraftCareerPathSegmentService draftCareerPathSegmentService)
        {
            this.repository = repository;
            this.draftCareerPathSegmentService = draftCareerPathSegmentService;
        }

        public async Task<bool> PingAsync()
        {
            return await repository.PingAsync().ConfigureAwait(false);
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
                ? await draftCareerPathSegmentService.GetSitefinityData(canonicalName.ToLowerInvariant()).ConfigureAwait(false)
                : await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<CareerPathSegmentModel> CreateAsync(CareerPathSegmentModel careerPathSegmentModel)
        {
            if (careerPathSegmentModel == null)
            {
                throw new ArgumentNullException(nameof(careerPathSegmentModel));
            }

            if (careerPathSegmentModel.Data == null)
            {
                careerPathSegmentModel.Data = new CareerPathSegmentDataModel();
            }

            careerPathSegmentModel.Updated = DateTime.UtcNow;

            var result = await repository.CreateAsync(careerPathSegmentModel).ConfigureAwait(false);

            return result == HttpStatusCode.Created
                ? await GetByIdAsync(careerPathSegmentModel.DocumentId).ConfigureAwait(false)
                : null;
        }

        public async Task<CareerPathSegmentModel> ReplaceAsync(CareerPathSegmentModel careerPathSegmentModel)
        {
            if (careerPathSegmentModel == null)
            {
                throw new ArgumentNullException(nameof(careerPathSegmentModel));
            }

            if (careerPathSegmentModel.Data == null)
            {
                careerPathSegmentModel.Data = new CareerPathSegmentDataModel();
            }

            careerPathSegmentModel.Updated = DateTime.UtcNow;

            var result = await repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel).ConfigureAwait(false);

            return result == HttpStatusCode.OK
                ? await GetByIdAsync(careerPathSegmentModel.DocumentId).ConfigureAwait(false)
                : null;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}

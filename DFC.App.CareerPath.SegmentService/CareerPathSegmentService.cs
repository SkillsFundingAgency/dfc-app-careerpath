using AutoMapper;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.SegmentService
{
    public class CareerPathSegmentService : ICareerPathSegmentService
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly IMapper mapper;

        public CareerPathSegmentService(
                                        ICosmosRepository<CareerPathSegmentModel> repository,
                                        IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService,
                                        IMapper mapper)
        {
            this.repository = repository;
            this.jobProfileSegmentRefreshService = jobProfileSegmentRefreshService;
            this.mapper = mapper;
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

        public async Task<CareerPathSegmentModel> GetByNameAsync(string canonicalName)
        {
            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> UpsertAsync(CareerPathSegmentModel careerPathSegmentModel)
        {
            if (careerPathSegmentModel == null)
            {
                throw new ArgumentNullException(nameof(careerPathSegmentModel));
            }

            if (careerPathSegmentModel.Data == null)
            {
                careerPathSegmentModel.Data = new CareerPathSegmentDataModel();
            }

            var result = await repository.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            if (result == HttpStatusCode.OK || result == HttpStatusCode.Created)
            {
                var refreshJobProfileSegmentServiceBusModel = mapper.Map<RefreshJobProfileSegmentServiceBusModel>(careerPathSegmentModel);
                await jobProfileSegmentRefreshService.SendMessageAsync(refreshJobProfileSegmentServiceBusModel).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}
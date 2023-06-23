using AutoMapper;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.Extensions.Logging;
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
        private readonly ILogService logService;

        public CareerPathSegmentService(
                                        ICosmosRepository<CareerPathSegmentModel> repository,
                                        IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService,
                                        IMapper mapper,
                                        ILogService logService)
        {
            this.repository = repository;
            this.jobProfileSegmentRefreshService = jobProfileSegmentRefreshService;
            this.mapper = mapper;
            this.logService = logService;
        }

        public async Task<bool> PingAsync()
        {
            logService.LogInformation($"{nameof(PingAsync)} has been called");

            return await repository.PingAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<CareerPathSegmentModel>> GetAllAsync()
        {
            logService.LogInformation($"{nameof(GetAllAsync)} has been called");

            return await repository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task<CareerPathSegmentModel> GetByIdAsync(Guid documentId)
        {
            logService.LogInformation($"{nameof(GetByIdAsync)} has been called");

            return await repository.GetAsync(d => d.DocumentId == documentId).ConfigureAwait(false);
        }

        public async Task<CareerPathSegmentModel> GetByNameAsync(string canonicalName)
        {
            logService.LogInformation($"{nameof(GetByNameAsync)} has been called");

            if (string.IsNullOrWhiteSpace(canonicalName))
            {
                throw new ArgumentNullException(nameof(canonicalName));
            }

            return await repository.GetAsync(d => d.CanonicalName == canonicalName.ToLowerInvariant()).ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> UpsertAsync(CareerPathSegmentModel careerPathSegmentModel)
        {
            logService.LogInformation($"{nameof(UpsertAsync)} has been called");

            if (careerPathSegmentModel == null)
            {
                throw new ArgumentNullException(nameof(careerPathSegmentModel));
            }

            if (careerPathSegmentModel.Data == null)
            {
                logService.LogInformation($"When calling {nameof(UpsertAsync)}, {nameof(careerPathSegmentModel.Data)} has returned null");

                careerPathSegmentModel.Data = new CareerPathSegmentDataModel();
            }

            var result = await repository.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            if (result == HttpStatusCode.OK || result == HttpStatusCode.Created)
            {
                logService.LogInformation($"{nameof(UpsertAsync)} has been called, returning 200 OK or 201 Created");

                var refreshJobProfileSegmentServiceBusModel = mapper.Map<RefreshJobProfileSegmentServiceBusModel>(careerPathSegmentModel);
                await jobProfileSegmentRefreshService.SendMessageAsync(refreshJobProfileSegmentServiceBusModel).ConfigureAwait(false);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(Guid documentId)
        {
            logService.LogInformation($"{nameof(DeleteAsync)} has been called");

            var result = await repository.DeleteAsync(documentId).ConfigureAwait(false);

            return result == HttpStatusCode.NoContent;
        }
    }
}
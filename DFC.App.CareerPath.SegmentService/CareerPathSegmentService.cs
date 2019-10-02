﻿using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.SegmentService
{
    public class CareerPathSegmentService : ICareerPathSegmentService
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegment> jobProfileSegmentRefreshService;

        public CareerPathSegmentService(ICosmosRepository<CareerPathSegmentModel> repository,
                                        IDraftCareerPathSegmentService draftCareerPathSegmentService,
                                        IJobProfileSegmentRefreshService<RefreshJobProfileSegment> jobProfileSegmentRefreshService)
        {
            this.repository = repository;
            this.draftCareerPathSegmentService = draftCareerPathSegmentService;
            this.jobProfileSegmentRefreshService = jobProfileSegmentRefreshService;
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
                var refreshJobProfileSegment = new RefreshJobProfileSegment
                {
                    JobProfileId = careerPathSegmentModel.DocumentId,
                    Segment = CareerPathSegmentModel.SegmentName,
                };

                await jobProfileSegmentRefreshService.SendMessageAsync(refreshJobProfileSegment).ConfigureAwait(false);
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

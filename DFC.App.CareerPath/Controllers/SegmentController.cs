﻿using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.Extensions;
using DFC.App.CareerPath.ViewModels;
using DFC.Logger.AppInsights.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Controllers
{
    public class SegmentController : Controller
    {
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly AutoMapper.IMapper mapper;
        private readonly ILogService logService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> refreshService;

        public SegmentController(ICareerPathSegmentService careerPathSegmentService, AutoMapper.IMapper mapper, ILogService logService, IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> refreshService)
        {
            this.careerPathSegmentService = careerPathSegmentService;
            this.mapper = mapper;
            this.logService = logService;
            this.refreshService = refreshService;
        }

        [HttpGet]
        [Route("{controller}")]
        public async Task<IActionResult> Index()
        {
            logService.LogInformation($"{nameof(Index)} has been called");

            var viewModel = new IndexViewModel();
            var careerPathSegmentModels = await careerPathSegmentService.GetAllAsync().ConfigureAwait(false);

            if (careerPathSegmentModels != null)
            {
                viewModel.Documents = (from a in careerPathSegmentModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                logService.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                logService.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("{controller}/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            logService.LogInformation($"{nameof(Document)} has been called with: {article}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByNameAsync(article).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(careerPathSegmentModel);

                logService.LogInformation($"{nameof(Document)} has succeeded for: {article}");

                return View(viewModel);
            }

            logService.LogWarning($"{nameof(Document)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpPost]
        [Route("{controller}/refreshDocuments")]
        public async Task<IActionResult> RefreshDocuments()
        {
            logService.LogInformation($"{nameof(RefreshDocuments)} has been called");

            var segmentModels = await careerPathSegmentService.GetAllAsync().ConfigureAwait(false);
            if (segmentModels != null)
            {
                var result = segmentModels
                    .OrderBy(x => x.CanonicalName)
                    .Select(x => mapper.Map<RefreshJobProfileSegmentServiceBusModel>(x))
                    .ToList();

                await refreshService.SendMessageListAsync(result).ConfigureAwait(false);

                logService.LogInformation($"{nameof(RefreshDocuments)} has succeeded");
                return Json(result);
            }

            logService.LogWarning($"{nameof(RefreshDocuments)} has returned with no results");
            return NoContent();
        }

        [HttpGet]
        [Route("{controller}/{documentId}/contents")]
        public async Task<IActionResult> Body(Guid documentId)
        {
            logService.LogInformation($"{nameof(Body)} has been called with: {documentId}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<BodyViewModel>(careerPathSegmentModel);

                logService.LogInformation($"{nameof(Body)} has succeeded for: {documentId}");

                var apiModel = mapper.Map<CareerPathAndProgressionApiModel>(careerPathSegmentModel.Data);

                return this.NegotiateContentResult(viewModel, apiModel);
            }

            logService.LogInformation($"{nameof(Body)} has returned no content for: {documentId}");

            return NoContent();
        }

        [HttpPost]
        [Route("{controller}")]
        public async Task<IActionResult> Post([FromBody]CareerPathSegmentModel careerPathSegmentModel)
        {
            logService.LogInformation($"{nameof(Post)} has been called");

            if (careerPathSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await careerPathSegmentService.GetByIdAsync(careerPathSegmentModel.DocumentId).ConfigureAwait(false);
            if (existingDocument != null)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            var response = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            logService.LogInformation($"{nameof(Post)} has created content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("{controller}")]
        public async Task<IActionResult> Put([FromBody]CareerPathSegmentModel careerPathSegmentModel)
        {
            logService.LogInformation($"{nameof(Put)} has been called");

            if (careerPathSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDocument = await careerPathSegmentService.GetByIdAsync(careerPathSegmentModel.DocumentId).ConfigureAwait(false);
            if (existingDocument == null)
            {
                logService.LogInformation($"{nameof(Put)}. Couldnt find document with Id {careerPathSegmentModel.DocumentId}");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (careerPathSegmentModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            careerPathSegmentModel.Etag = existingDocument.Etag;
            careerPathSegmentModel.SocLevelTwo = existingDocument.SocLevelTwo;

            var response = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            logService.LogInformation($"{nameof(Put)} has updated content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("{controller}/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logService.LogInformation($"{nameof(Delete)} has been called");

            var isDeleted = await careerPathSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            if (isDeleted)
            {
                logService.LogInformation($"{nameof(Delete)} has deleted content for: {documentId}");
                return Ok();
            }
            else
            {
                logService.LogWarning($"{nameof(Document)} has returned no content for: {documentId}");
                return NotFound();
            }
        }
    }
}
using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Extensions;
using DFC.App.CareerPath.ViewModels;
using Microsoft.ApplicationInsights.DataContracts;
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

        public SegmentController(ICareerPathSegmentService careerPathSegmentService, AutoMapper.IMapper mapper, ILogService logService)
        {
            this.careerPathSegmentService = careerPathSegmentService;
            this.mapper = mapper;
            this.logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logService.LogMessage($"{nameof(Index)} has been called");

            var viewModel = new IndexViewModel();
            var careerPathSegmentModels = await careerPathSegmentService.GetAllAsync().ConfigureAwait(false);

            if (careerPathSegmentModels != null)
            {
                viewModel.Documents = (from a in careerPathSegmentModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                logService.LogMessage($"{nameof(Index)} has succeeded");
            }
            else
            {
                logService.LogMessage($"{nameof(Index)} has returned with no results", SeverityLevel.Warning);
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("{controller}/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            logService.LogMessage($"{nameof(Document)} has been called with: {article}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByNameAsync(article).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(careerPathSegmentModel);

                logService.LogMessage($"{nameof(Document)} has succeeded for: {article}");

                return View(viewModel);
            }

            logService.LogMessage($"{nameof(Document)} has returned no content for: {article}", SeverityLevel.Warning);

            return NoContent();
        }

        [HttpGet]
        [Route("{controller}/{documentId}/contents")]
        public async Task<IActionResult> Body(Guid documentId)
        {
            logService.LogMessage($"{nameof(Body)} has been called with: {documentId}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<BodyViewModel>(careerPathSegmentModel);

                logService.LogMessage($"{nameof(Body)} has succeeded for: {documentId}");

                var apiModel = mapper.Map<CareerPathAndProgressionApiModel>(careerPathSegmentModel.Data);

                return this.NegotiateContentResult(viewModel, apiModel);
            }

            logService.LogMessage($"{nameof(Body)} has returned no content for: {documentId}");

            return NoContent();
        }

        [HttpPost]
        [Route("{controller}")]
        public async Task<IActionResult> Post([FromBody]CareerPathSegmentModel careerPathSegmentModel)
        {
            logService.LogMessage($"{nameof(Post)} has been called");

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

            logService.LogMessage($"{nameof(Post)} has created content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("{controller}")]
        public async Task<IActionResult> Put([FromBody]CareerPathSegmentModel careerPathSegmentModel)
        {
            logService.LogMessage($"{nameof(Put)} has been called");

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
                logService.LogMessage($"{nameof(Put)}. Couldnt find document with Id {careerPathSegmentModel.DocumentId}");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (careerPathSegmentModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            careerPathSegmentModel.Etag = existingDocument.Etag;
            careerPathSegmentModel.SocLevelTwo = existingDocument.SocLevelTwo;

            var response = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            logService.LogMessage($"{nameof(Put)} has updated content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("{controller}/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logService.LogMessage($"{nameof(Delete)} has been called");

            var isDeleted = await careerPathSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            if (isDeleted)
            {
                logService.LogMessage($"{nameof(Delete)} has deleted content for: {documentId}");
                return Ok();
            }
            else
            {
                logService.LogMessage($"{nameof(Document)} has returned no content for: {documentId}", SeverityLevel.Warning);
                return NotFound();
            }
        }
    }
}
using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Common.Services;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.PatchModels;
using DFC.App.CareerPath.Extensions;
using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Controllers
{
    public class SegmentController : Controller
    {
        private readonly ILogger<SegmentController> logger;
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly AutoMapper.IMapper mapper;
        private readonly ILogService logService;

        public SegmentController(ILogger<SegmentController> logger, ICareerPathSegmentService careerPathSegmentService, AutoMapper.IMapper mapper, ILogService logService)
        {
            this.logger = logger;
            this.careerPathSegmentService = careerPathSegmentService;
            this.mapper = mapper;
            this.logService = logService;
        }

        [HttpGet]
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
                logger.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("{controller}/{article}")]
        public async Task<IActionResult> Document(string article)
        {
            logService.LogInformation($"{nameof(Document)} has been called with: {article}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByNameAsync(article, Request.IsDraftRequest()).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(careerPathSegmentModel);

                logService.LogInformation($"{nameof(Document)} has succeeded for: {article}");

                return View(viewModel);
            }

            logger.LogWarning($"{nameof(Document)} has returned no content for: {article}");

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

            var response = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            logService.LogInformation($"{nameof(Put)} has updated content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("{controller}/{documentId}/content-type/markup")]
        public async Task<IActionResult> Patch([FromBody]CareerPathPatchSegmentModel careerPathPatchSegmentModel, Guid documentId)
        {
            logService.LogInformation($"{nameof(Patch)} has been called");

            if (careerPathPatchSegmentModel == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var careerPathSegmentModel = await careerPathSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (careerPathSegmentModel == null)
            {
                logger.LogWarning($"{nameof(Document)} has returned no content for: {documentId} - creating new document");

                careerPathSegmentModel = new CareerPathSegmentModel
                {
                    DocumentId = documentId,
                    SocLevelTwo = careerPathPatchSegmentModel.SocLevelTwo,
                    Data = new CareerPathSegmentDataModel(),
                };
            }

            careerPathSegmentModel.CanonicalName = careerPathPatchSegmentModel.CanonicalName;
            careerPathSegmentModel.Data.Markup = careerPathPatchSegmentModel.Data.Markup;

            var response = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            logService.LogInformation($"{nameof(Patch)} has patched content for: {careerPathSegmentModel.CanonicalName}");

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
                logger.LogWarning($"{nameof(Document)} has returned no content for: {documentId}");
                return NotFound();
            }
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
    }
}
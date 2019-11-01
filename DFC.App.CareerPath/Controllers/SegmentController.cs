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

        public SegmentController(ILogger<SegmentController> logger, ICareerPathSegmentService careerPathSegmentService, AutoMapper.IMapper mapper)
        {
            this.logger = logger;
            this.careerPathSegmentService = careerPathSegmentService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            logger.LogInformation($"{nameof(Index)} has been called");

            var viewModel = new IndexViewModel();
            var careerPathSegmentModels = await careerPathSegmentService.GetAllAsync().ConfigureAwait(false);

            if (careerPathSegmentModels != null)
            {
                viewModel.Documents = (from a in careerPathSegmentModels.OrderBy(o => o.CanonicalName)
                                       select mapper.Map<IndexDocumentViewModel>(a)).ToList();

                logger.LogInformation($"{nameof(Index)} has succeeded");
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
            logger.LogInformation($"{nameof(Document)} has been called with: {article}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByNameAsync(article, Request.IsDraftRequest()).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(careerPathSegmentModel);

                logger.LogInformation($"{nameof(Document)} has succeeded for: {article}");

                return View(viewModel);
            }

            logger.LogWarning($"{nameof(Document)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpPost]
        [Route("{controller}")]
        public async Task<IActionResult> Post([FromBody]CareerPathSegmentModel careerPathSegmentModel)
        {
            logger.LogInformation($"{nameof(Post)} has been called");

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

            logger.LogInformation($"{nameof(Post)} has created content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPut]
        [Route("{controller}")]
        public async Task<IActionResult> Put([FromBody]CareerPathSegmentModel careerPathSegmentModel)
        {
            logger.LogInformation($"{nameof(Put)} has been called");

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
                logger.LogInformation($"{nameof(Put)}. Couldnt find document with Id {careerPathSegmentModel.DocumentId}");
                return new StatusCodeResult((int)HttpStatusCode.NotFound);
            }

            if (careerPathSegmentModel.SequenceNumber <= existingDocument.SequenceNumber)
            {
                return new StatusCodeResult((int)HttpStatusCode.AlreadyReported);
            }

            var response = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            logger.LogInformation($"{nameof(Put)} has updated content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpPatch]
        [Route("{controller}/{documentId}/content-type/markup")]
        public async Task<IActionResult> Patch([FromBody]CareerPathPatchSegmentModel careerPathPatchSegmentModel, Guid documentId)
        {
            logger.LogInformation($"{nameof(Patch)} has been called");

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

            logger.LogInformation($"{nameof(Patch)} has patched content for: {careerPathSegmentModel.CanonicalName}");

            return new StatusCodeResult((int)response);
        }

        [HttpDelete]
        [Route("{controller}/{documentId}")]
        public async Task<IActionResult> Delete(Guid documentId)
        {
            logger.LogInformation($"{nameof(Delete)} has been called");

            var isDeleted = await careerPathSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            if (isDeleted)
            {
                logger.LogInformation($"{nameof(Delete)} has deleted content for: {documentId}");
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
            logger.LogInformation($"{nameof(Body)} has been called with: {documentId}");

            var careerPathSegmentModel = await careerPathSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (careerPathSegmentModel != null)
            {
                var viewModel = mapper.Map<BodyViewModel>(careerPathSegmentModel);

                logger.LogInformation($"{nameof(Body)} has succeeded for: {documentId}");

                return this.NegotiateContentResult(viewModel, careerPathSegmentModel.Data);
            }

            logger.LogWarning($"{nameof(Body)} has returned no content for: {documentId}");

            return NoContent();
        }
    }
}
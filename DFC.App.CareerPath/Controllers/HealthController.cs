﻿using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Extensions;
using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.Controllers
{
    public class HealthController : Controller
    {
        private readonly ILogger<HealthController> logger;
        private readonly ICareerPathSegmentService careerPathSegmentService;

        public HealthController(ILogger<HealthController> logger, ICareerPathSegmentService careerPathSegmentService)
        {
            this.logger = logger;
            this.careerPathSegmentService = careerPathSegmentService;
        }

        [HttpGet]
        [Route("health")]
        public async Task<IActionResult> Health()
        {
            string resourceName = typeof(Program).Namespace;
            string message;

            logger.LogInformation($"{nameof(Health)} has been called");

            try
            {
                var isHealthy = await careerPathSegmentService.PingAsync().ConfigureAwait(false);

                if (isHealthy)
                {
                    message = "Document store is available";
                    logger.LogInformation($"{nameof(Health)} responded with: {resourceName} - {message}");

                    var viewModel = CreateHealthViewModel(resourceName, message);

                    return this.NegotiateContentResult(viewModel);
                }

                message = $"Ping to {resourceName} has failed";
                logger.LogError($"{nameof(Health)}: {message}");
            }
            catch (Exception ex)
            {
                message = $"{resourceName} exception: {ex.Message}";
                logger.LogError(ex, $"{nameof(Health)}: {message}");
            }

            return StatusCode((int)HttpStatusCode.ServiceUnavailable);
        }

        [HttpGet]
        public IActionResult Ping()
        {
            logger.LogInformation($"{nameof(Ping)} has been called");

            return Ok();
        }

        private static HealthViewModel CreateHealthViewModel(string resourceName, string message)
        {
            return new HealthViewModel
            {
                HealthItems = new List<HealthItemViewModel>
                {
                    new HealthItemViewModel
                    {
                        Service = resourceName,
                        Message = message,
                    },
                },
            };
        }
    }
}
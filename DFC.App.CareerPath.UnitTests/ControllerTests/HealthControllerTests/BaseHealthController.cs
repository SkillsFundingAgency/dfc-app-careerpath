using DFC.App.CareerPath.Controllers;
using DFC.App.CareerPath.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.HealthControllerTests
{
    public abstract class BaseHealthController
    {
        public BaseHealthController()
        {
            FakeCareerPathSegmentService = A.Fake<ICareerPathSegmentService>();
            FakeLogger = A.Fake<ILogger<HealthController>>();
        }

        protected ICareerPathSegmentService FakeCareerPathSegmentService { get; }

        protected ILogger<HealthController> FakeLogger { get; }

        protected HealthController BuildHealthController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new HealthController(FakeLogger, FakeCareerPathSegmentService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}
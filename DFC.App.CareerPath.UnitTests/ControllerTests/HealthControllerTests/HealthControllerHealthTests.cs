using DFC.App.CareerPath.ViewModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.HealthControllerTests
{
    [Trait("Health Controller", "Health Tests")]
    public class HealthControllerHealthTests : BaseHealthController
    {
        [Fact]
        public async Task HealthControllerHealthReturnsSuccessWhenhealthy()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            A.CallTo(() => FakeCareerPathSegmentService.PingAsync()).Returns(true);

            // Act
            var result = await controller.Health().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.PingAsync()).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HealthViewModel>(jsonResult.Value);

            model.HealthItems.Count.Should().BeGreaterThan(0);
            model.HealthItems.First().Service.Should().NotBeNullOrWhiteSpace();
            model.HealthItems.First().Message.Should().NotBeNullOrWhiteSpace();

            controller.Dispose();
        }

        [Fact]
        public async Task HealthControllerHealthReturnsServiceUnavailableWhenUnhealthy()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            A.CallTo(() => FakeCareerPathSegmentService.PingAsync()).Returns(false);

            // Act
            var result = await controller.Health().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.PingAsync()).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);

            controller.Dispose();
        }

        [Fact]
        public async Task HealthControllerHealthReturnsServiceUnavailableWhenException()
        {
            // Arrange
            var controller = BuildHealthController(MediaTypeNames.Application.Json);

            A.CallTo(() => FakeCareerPathSegmentService.PingAsync()).Throws<Exception>();

            // Act
            var result = await controller.Health().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.PingAsync()).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.ServiceUnavailable, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
using DFC.App.CareerPath.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Post Tests")]
    public class SegmentControllerPostTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task ReturnsAlreadyReportedForCreate(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.Created);
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(careerPathSegmentModel);

            // Act
            var result = await controller.Post(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustNotHaveHappened();

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.AlreadyReported, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task ReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.OK);
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns<CareerPathSegmentModel>(null);

            // Act
            var result = await controller.Post(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.OK, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task ReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            CareerPathSegmentModel careerPathSegmentModel = null;
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Post(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task ReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = new CareerPathSegmentModel();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Post(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
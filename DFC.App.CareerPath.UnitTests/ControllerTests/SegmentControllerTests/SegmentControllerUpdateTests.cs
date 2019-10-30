using DFC.App.CareerPath.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Update Tests")]
    public class SegmentControllerUpdateTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void ReturnsCreatedForCreate(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            careerPathSegmentModel.SequenceNumber = int.MaxValue;
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await controller.Put(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.Created, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void ReturnsOKForUpdate(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            careerPathSegmentModel.SequenceNumber = int.MaxValue;
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.Put(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.OK, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void ReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            CareerPathSegmentModel careerPathSegmentModel = null;
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.Put(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void ReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = new CareerPathSegmentModel();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.Put(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}

using DFC.App.CareerPath.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Create or Update Tests")]
    public class SegmentControllerCreateOrUpdateTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerCreateOrUpdateReturnsSuccessForCreate(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await controller.CreateOrUpdate(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.Created, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerCreateOrUpdateReturnsSuccessForUpdate(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await controller.CreateOrUpdate(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);

            A.Equals((int)HttpStatusCode.OK, statusCodeResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerCreateOrUpdateReturnsBadResultWhenModelIsNull(string mediaTypeName)
        {
            // Arrange
            CareerPathSegmentModel careerPathSegmentModel = null;
            var controller = BuildSegmentController(mediaTypeName);

            // Act
            var result = await controller.CreateOrUpdate(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestResult>(result);

            A.Equals((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerCreateOrUpdateReturnsBadResultWhenModelIsInvalid(string mediaTypeName)
        {
            // Arrange
            var careerPathSegmentModel = new CareerPathSegmentModel();
            var controller = BuildSegmentController(mediaTypeName);

            controller.ModelState.AddModelError(string.Empty, "Model is not valid");

            // Act
            var result = await controller.CreateOrUpdate(careerPathSegmentModel).ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<BadRequestObjectResult>(result);

            A.Equals((int)HttpStatusCode.BadRequest, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}

using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Body Tests")]
    public class SegmentControllerBodyTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task SegmentControllerBodyHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var expectedResult = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            expectedResult.Data = A.Fake<CareerPathSegmentDataModel>();

            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<BodyViewModel>(A<CareerPathSegmentModel>.Ignored)).Returns(A.Fake<BodyViewModel>());

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<BodyViewModel>(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<BodyViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async Task SegmentControllerBodyJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var expectedResult = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);
            var apiModel = GetCareerPathAndProgressionApiModel();

            expectedResult.Data = A.Fake<CareerPathSegmentDataModel>();

            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<CareerPathAndProgressionApiModel>(A<CareerPathSegmentDataModel>.Ignored)).Returns(apiModel);

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<BodyViewModel>(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var jsonResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<CareerPathAndProgressionApiModel>(jsonResult.Value);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(InvalidMediaTypes))]
        public async Task SegmentControllerBodyReturnsNotAcceptable(string mediaTypeName)
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var expectedResult = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            expectedResult.Data = A.Fake<CareerPathSegmentDataModel>();

            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);
            A.CallTo(() => FakeMapper.Map<BodyViewModel>(A<CareerPathSegmentModel>.Ignored)).Returns(A.Fake<BodyViewModel>());

            // Act
            var result = await controller.Body(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeMapper.Map<BodyViewModel>(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<StatusCodeResult>(result);

            Assert.Equal((int)HttpStatusCode.NotAcceptable, statusResult.StatusCode);

            controller.Dispose();
        }

        private CareerPathAndProgressionApiModel GetCareerPathAndProgressionApiModel()
        {
            return new CareerPathAndProgressionApiModel
            {
                CareerPathAndProgression = new List<string> { "Path1", "Path2", "Path3" },
            };
        }
    }
}
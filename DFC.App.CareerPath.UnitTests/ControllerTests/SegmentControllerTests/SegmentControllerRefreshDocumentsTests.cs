using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    public class SegmentControllerRefreshDocumentsTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task ReturnsSuccessForHtmlMediaType(string mediaTypeName)
        {
            // Arrange
            var controller = BuildSegmentController(mediaTypeName);
            var dbModels = GetSegmentModels();

            A.CallTo(() => FakeCareerPathSegmentService.GetAllAsync()).Returns(dbModels);

            // Act
            var result = await controller.RefreshDocuments().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobProfileSegmentRefreshService.SendMessageListAsync(A<List<RefreshJobProfileSegmentServiceBusModel>>.Ignored)).MustHaveHappenedOnceExactly();
            var res = Assert.IsType<JsonResult>(result);
            Assert.Equal(dbModels.Count, (res.Value as List<RefreshJobProfileSegmentServiceBusModel>).Count);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task ReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            List<CareerPathSegmentModel> expectedResult = null;
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.GetAllAsync()).Returns(expectedResult);

            // Act
            var result = await controller.RefreshDocuments().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeJobProfileSegmentRefreshService.SendMessageListAsync(A<List<RefreshJobProfileSegmentServiceBusModel>>.Ignored)).MustNotHaveHappened();
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.CallTo(() => FakeLogService.LogWarning(A<string>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal((int)HttpStatusCode.NoContent, statusResult.StatusCode);

            controller.Dispose();
        }

        private List<CareerPathSegmentModel> GetSegmentModels()
        {
            return new List<CareerPathSegmentModel>
            {
                new CareerPathSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile1",
                },
                new CareerPathSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile2",
                },
                new CareerPathSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile3",
                },
                new CareerPathSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile4",
                },
                new CareerPathSegmentModel
                {
                    DocumentId = Guid.NewGuid(),
                    CanonicalName = "JobProfile5",
                },
            };
        }
    }
}
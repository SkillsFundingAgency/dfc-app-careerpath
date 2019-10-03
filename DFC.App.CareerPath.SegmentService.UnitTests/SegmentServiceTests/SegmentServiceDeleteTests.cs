using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.DraftSegmentService;
using FakeItEasy;
using System;
using System.Net;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "Delete Tests")]
    public class SegmentServiceDeleteTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;

        public SegmentServiceDeleteTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            draftCareerPathSegmentService = A.Fake<DraftCareerPathSegmentService>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            careerPathSegmentService = new CareerPathSegmentService(repository, draftCareerPathSegmentService, jobProfileSegmentRefreshService);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsSuccessWhenSegmentDeleted()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = true;

            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.NoContent);

            // act
            var result = careerPathSegmentService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsNullWhenSegmentNotDeleted()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = careerPathSegmentService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.NotFound);

            // act
            var result = careerPathSegmentService.DeleteAsync(documentId).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}

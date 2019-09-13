using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
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
        private readonly ICareerPathSegmentService careerPathSegmentService;

        public SegmentServiceDeleteTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            draftCareerPathSegmentService = A.Fake<DraftCareerPathSegmentService>();
            careerPathSegmentService = new CareerPathSegmentService(repository, draftCareerPathSegmentService);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsSuccessWhenSegmentDeleted()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            const int partitionKey = 0;
            var expectedResult = true;

            A.CallTo(() => repository.DeleteAsync(documentId, partitionKey)).Returns(HttpStatusCode.NoContent);

            // act
            var result = careerPathSegmentService.DeleteAsync(documentId, partitionKey).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId, partitionKey)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsNullWhenSegmentNotDeleted()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            const int partitionKey = 0;
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(documentId, partitionKey)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = careerPathSegmentService.DeleteAsync(documentId, partitionKey).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId, partitionKey)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            const int partitionKey = 0;
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(documentId, partitionKey)).Returns(HttpStatusCode.FailedDependency);

            // act
            var result = careerPathSegmentService.DeleteAsync(documentId, partitionKey).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(documentId, partitionKey)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}

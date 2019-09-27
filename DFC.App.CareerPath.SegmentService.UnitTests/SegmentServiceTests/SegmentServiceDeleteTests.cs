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
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = true;

            A.CallTo(() => repository.DeleteAsync(careerPathSegmentModel)).Returns(HttpStatusCode.NoContent);

            // act
            var result = careerPathSegmentService.DeleteAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsNullWhenSegmentNotDeleted()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(careerPathSegmentModel)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = careerPathSegmentService.DeleteAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(careerPathSegmentModel)).Returns(HttpStatusCode.FailedDependency);

            // act
            var result = careerPathSegmentService.DeleteAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.DeleteAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}

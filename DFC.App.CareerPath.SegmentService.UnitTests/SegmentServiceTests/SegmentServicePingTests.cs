using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using FakeItEasy;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "Ping / Health Tests")]
    public class SegmentServicePingTests
    {
        [Fact]
        public void CareerPathSegmentServicePingReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            var expectedResult = true;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var careerPathSegmentService = new CareerPathSegmentService(repository, A.Fake<IDraftCareerPathSegmentService>());

            // act
            var result = careerPathSegmentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServicePingReturnsFalseWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<CareerPathSegmentModel>>();
            var expectedResult = false;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var careerPathSegmentService = new CareerPathSegmentService(repository, A.Fake<IDraftCareerPathSegmentService>());

            // act
            var result = careerPathSegmentService.PingAsync().Result;

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
using AutoMapper;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "Ping / Health Tests")]
    public class SegmentServicePingTests
    {
        [Fact]
        public async Task CareerPathSegmentServicePingReturnsSuccess()
        {
            // arrange
            var repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            var logService = A.Fake<ILogService>();
            var expectedResult = true;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var careerPathSegmentService = new CareerPathSegmentService(repository, A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>(), A.Fake<IMapper>(), logService);

            // act
            var result = await careerPathSegmentService.PingAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CareerPathSegmentServicePingReturnsFalseWhenMissingRepository()
        {
            // arrange
            var repository = A.Dummy<ICosmosRepository<CareerPathSegmentModel>>();
            var logService = A.Fake<ILogService>();
            var expectedResult = false;

            A.CallTo(() => repository.PingAsync()).Returns(expectedResult);

            var careerPathSegmentService = new CareerPathSegmentService(repository, A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>(), A.Fake<IMapper>(), logService);

            // act
            var result = await careerPathSegmentService.PingAsync().ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.PingAsync()).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }
    }
}
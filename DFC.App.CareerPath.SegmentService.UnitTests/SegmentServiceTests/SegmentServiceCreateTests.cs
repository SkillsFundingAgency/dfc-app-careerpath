using AutoMapper;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.Logger.AppInsights.Contracts;
using FakeItEasy;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "Create Tests")]
    public class SegmentServiceCreateTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly ILogService logService;

        public SegmentServiceCreateTests()
        {
            var mapper = A.Fake<IMapper>();
            logService = A.Fake<ILogService>();
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            careerPathSegmentService = new CareerPathSegmentService(repository, jobProfileSegmentRefreshService, mapper, logService);
        }

        [Fact]
        public async Task CareerPathSegmentServiceCreateReturnsSuccessWhenSegmentCreated()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.Created);

            // act
            var result = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegmentServiceBusModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(HttpStatusCode.Created, result);
        }

        [Fact]
        public async Task CareerPathSegmentServiceCreateReturnsArgumentNullExceptionWhenNullIsUsedAsync()
        {
            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await careerPathSegmentService.UpsertAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'careerPathSegmentModel')", exceptionResult.Message);
        }

        [Fact]
        public async Task CareerPathSegmentServiceCreateReturnsStatusWhenSegmentNotCreated()
        {
            // arrange
            var createOrUdateCareerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = HttpStatusCode.BadRequest;

            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(expectedResult);

            // act
            var result = await careerPathSegmentService.UpsertAsync(createOrUdateCareerPathSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegmentServiceBusModel>.Ignored)).MustNotHaveHappened();
            Assert.Equal(expectedResult, result);
        }
    }
}
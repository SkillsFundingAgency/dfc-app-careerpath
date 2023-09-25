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
    [Trait("Segment Service", "Update Tests")]
    public class SegmentServiceUpdateTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly IMapper mapper;
        private readonly ILogService logService;

        public SegmentServiceUpdateTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            logService = A.Fake<ILogService>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            mapper = A.Fake<IMapper>();
            careerPathSegmentService = new CareerPathSegmentService(repository, jobProfileSegmentRefreshService, mapper, logService);
        }

        [Fact]
        public async Task CareerPathSegmentServiceUpdateReturnsSuccessWhenSegmentReplaced()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = HttpStatusCode.OK;
            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).Returns(HttpStatusCode.OK);

            // act
            var result = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegmentServiceBusModel>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CareerPathSegmentServiceUpdateReturnsArgumentNullExceptionWhenNullParamIsUsed()
        {
            // arrange
            CareerPathSegmentModel careerPathSegmentModel = null;

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null. (Parameter 'careerPathSegmentModel')", exceptionResult.Message);
        }

        [Fact]
        public async Task CareerPathSegmentServiceUpdateReturnsStatusWhenSegmentNotReplaced()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = HttpStatusCode.BadRequest;
            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegmentServiceBusModel>.Ignored)).MustNotHaveHappened();
            Assert.Equal(expectedResult, result);
        }
    }
}
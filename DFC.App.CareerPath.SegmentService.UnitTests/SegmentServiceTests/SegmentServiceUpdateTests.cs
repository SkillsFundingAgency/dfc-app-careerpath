using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.DraftSegmentService;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "Update Tests")]
    public class SegmentServiceUpdateTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegment> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;

        public SegmentServiceUpdateTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            draftCareerPathSegmentService = A.Fake<DraftCareerPathSegmentService>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegment>>();
            careerPathSegmentService = new CareerPathSegmentService(repository, draftCareerPathSegmentService, jobProfileSegmentRefreshService);
        }

        [Fact]
        public void CareerPathSegmentServiceUpdateReturnsSuccessWhenSegmentReplaced()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).Returns(HttpStatusCode.OK);
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegment>.Ignored));

            // act
            var result = careerPathSegmentService.UpsertAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegment>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task CareerPathSegmentServiceUpdateReturnsArgumentNullExceptionWhenNullParamIsUsed()
        {
            // arrange
            CareerPathSegmentModel careerPathSegmentModel = null;

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await careerPathSegmentService.UpsertAsync(careerPathSegmentModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: careerPathSegmentModel", exceptionResult.Message);
        }

        [Fact]
        public void CareerPathSegmentServiceUpdateReturnsOkWhenSegmentNotReplaced()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = HttpStatusCode.OK;

            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).Returns(HttpStatusCode.BadRequest);
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegment>.Ignored));

            // act
            var result = careerPathSegmentService.UpsertAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => jobProfileSegmentRefreshService.SendMessageAsync(A<RefreshJobProfileSegment>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}

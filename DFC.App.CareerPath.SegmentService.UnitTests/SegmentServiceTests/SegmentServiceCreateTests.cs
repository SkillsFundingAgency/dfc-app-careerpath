using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.DraftSegmentService;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "Create Tests")]
    public class SegmentServiceCreateTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;
        private readonly ICareerPathSegmentService careerPathSegmentService;

        public SegmentServiceCreateTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            draftCareerPathSegmentService = A.Fake<DraftCareerPathSegmentService>();
            careerPathSegmentService = new CareerPathSegmentService(repository, draftCareerPathSegmentService);
        }

        [Fact]
        public void CareerPathSegmentServiceCreateReturnsSuccessWhenSegmentCreated()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = A.Fake<CareerPathSegmentModel>();

            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.Created);

            // act
            var result = careerPathSegmentService.UpsertAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task CareerPathSegmentServiceCreateReturnsArgumentNullExceptionWhenNullIsUsedAsync()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await careerPathSegmentService.UpsertAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: careerPathSegmentModel", exceptionResult.Message);
        }

        [Fact]
        public void CareerPathSegmentServiceCreateReturnsNullWhenSegmentNotCreated()
        {
            // arrange
            var createOrUdateCareerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = A.Dummy<CareerPathSegmentModel>();

            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = careerPathSegmentService.UpsertAsync(createOrUdateCareerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceCreateReturnsNullWhenMissingRepository()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            CareerPathSegmentModel expectedResult = null;

            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).Returns(HttpStatusCode.FailedDependency);

            // act
            var result = careerPathSegmentService.UpsertAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpsertAsync(A<CareerPathSegmentModel>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}

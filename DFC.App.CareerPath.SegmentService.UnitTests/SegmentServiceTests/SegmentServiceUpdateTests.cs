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
    [Trait("Profile Service", "Update Tests")]
    public class SegmentServiceUpdateTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;
        private readonly ICareerPathSegmentService careerPathSegmentService;

        public SegmentServiceUpdateTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            draftCareerPathSegmentService = A.Fake<DraftCareerPathSegmentService>();
            careerPathSegmentService = new CareerPathSegmentService(repository, draftCareerPathSegmentService);
        }

        [Fact]
        public void CareerPathSegmentServiceUpdateReturnsSuccessWhenProfileReplaced()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = A.Fake<CareerPathSegmentModel>();

            A.CallTo(() => repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel)).Returns(HttpStatusCode.OK);
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = careerPathSegmentService.ReplaceAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task CareerPathSegmentServiceUpdateReturnsArgumentNullExceptionWhenNullParamIsUsed()
        {
            // arrange
            CareerPathSegmentModel careerPathSegmentModel = null;

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await careerPathSegmentService.ReplaceAsync(careerPathSegmentModel).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: careerPathSegmentModel", exceptionResult.Message);
        }

        [Fact]
        public void CareerPathSegmentServiceUpdateReturnsNullWhenProfileNotReplaced()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            var expectedResult = A.Dummy<CareerPathSegmentModel>();

            A.CallTo(() => repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = careerPathSegmentService.ReplaceAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public void CareerPathSegmentServiceUpdateReturnsNullWhenMissingRepository()
        {
            // arrange
            var careerPathSegmentModel = A.Fake<CareerPathSegmentModel>();
            CareerPathSegmentModel expectedResult = null;

            A.CallTo(() => repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel)).Returns(HttpStatusCode.FailedDependency);

            // act
            var result = careerPathSegmentService.ReplaceAsync(careerPathSegmentModel).Result;

            // assert
            A.CallTo(() => repository.UpdateAsync(careerPathSegmentModel.DocumentId, careerPathSegmentModel)).MustHaveHappenedOnceExactly();
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustNotHaveHappened();
            A.Equals(result, expectedResult);
        }
    }
}

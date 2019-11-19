using AutoMapper;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "GetByName Tests")]
    public class SegmentServiceGetByNameTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly IMapper mapper;

        public SegmentServiceGetByNameTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            mapper = A.Fake<IMapper>();
            careerPathSegmentService = new CareerPathSegmentService(repository, jobProfileSegmentRefreshService, mapper);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsSuccess()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var expectedResult = A.Fake<CareerPathSegmentModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await careerPathSegmentService.GetByNameAsync("article-name").ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsArgumentNullExceptionWhenNullIsUsed()
        {
            // arrange

            // act
            var exceptionResult = await Assert.ThrowsAsync<ArgumentNullException>(async () => await careerPathSegmentService.GetByNameAsync(null).ConfigureAwait(false)).ConfigureAwait(false);

            // assert
            Assert.Equal("Value cannot be null.\r\nParameter name: canonicalName", exceptionResult.Message);
        }

        [Fact]
        public async Task SegmentServiceGetByNameReturnsNullWhenMissingInRepository()
        {
            // arrange
            CareerPathSegmentModel expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await careerPathSegmentService.GetByNameAsync("article-name").ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
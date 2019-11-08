using AutoMapper;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.DraftSegmentService;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.CareerPath.SegmentService.UnitTests.SegmentServiceTests
{
    [Trait("Segment Service", "GetById Tests")]
    public class SegmentServiceGetByIdTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IDraftCareerPathSegmentService draftCareerPathSegmentService;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly IMapper mapper;

        public SegmentServiceGetByIdTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            draftCareerPathSegmentService = A.Fake<DraftCareerPathSegmentService>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            mapper = A.Fake<IMapper>();
            careerPathSegmentService = new CareerPathSegmentService(repository, draftCareerPathSegmentService, jobProfileSegmentRefreshService, mapper);
        }

        [Fact]
        public async Task SegmentServiceGetByIdReturnsSuccess()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            var expectedResult = A.Fake<CareerPathSegmentModel>();

            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await careerPathSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }

        [Fact]
        public async Task SegmentServiceGetByIdReturnsNullWhenMissingInRepository()
        {
            // arrange
            Guid documentId = Guid.NewGuid();
            CareerPathSegmentModel expectedResult = null;

            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).Returns(expectedResult);

            // act
            var result = await careerPathSegmentService.GetByIdAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.GetAsync(A<Expression<Func<CareerPathSegmentModel, bool>>>.Ignored)).MustHaveHappenedOnceExactly();
            A.Equals(result, expectedResult);
        }
    }
}
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
    [Trait("Segment Service", "Delete Tests")]
    public class SegmentServiceDeleteTests
    {
        private readonly ICosmosRepository<CareerPathSegmentModel> repository;
        private readonly IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel> jobProfileSegmentRefreshService;
        private readonly ICareerPathSegmentService careerPathSegmentService;
        private readonly IMapper mapper;
        private readonly ILogService logService;

        public SegmentServiceDeleteTests()
        {
            repository = A.Fake<ICosmosRepository<CareerPathSegmentModel>>();
            logService = A.Fake<ILogService>();
            jobProfileSegmentRefreshService = A.Fake<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            mapper = A.Fake<IMapper>();
            careerPathSegmentService = new CareerPathSegmentService(repository, jobProfileSegmentRefreshService, mapper, logService);
        }

        [Fact]
        public async Task CareerPathSegmentServiceDeleteReturnsSuccessWhenSegmentDeleted()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = true;

            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.NoContent);

            // act
            var result = await careerPathSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CareerPathSegmentServiceDeleteReturnsNullWhenSegmentNotDeleted()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.BadRequest);

            // act
            var result = await careerPathSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CareerPathSegmentServiceDeleteReturnsFalseWhenMissingRepository()
        {
            // arrange
            var documentId = Guid.NewGuid();
            var expectedResult = false;

            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.NotFound);

            // act
            var result = await careerPathSegmentService.DeleteAsync(documentId).ConfigureAwait(false);

            // assert
            A.CallTo(() => repository.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.Equal(expectedResult, result);
        }
    }
}
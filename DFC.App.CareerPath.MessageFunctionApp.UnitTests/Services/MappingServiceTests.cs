using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using DFC.App.JobProfileTasks.MessageFunctionApp.AutoMapperProfiles;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using Xunit;

namespace DFC.App.CareerPath.MessageFunctionApp.UnitTests.Services
{
    public class MappingServiceTests
    {
        private const int SequenceNumber = 123;
        private const string TestJobName = "Test Job name";
        private const string SocCodeId = "99";
        private const string MarkupString = "<h1>Some Markup</h1>";

        private static readonly Guid JobProfileId = Guid.NewGuid();
        private readonly IMappingService mappingService;
        private readonly DateTime lastModified = DateTime.UtcNow.AddDays(-1);

        public MappingServiceTests()
        {
            var config = new MapperConfiguration(opts => { opts.AddProfile(new CareerPathsProfile()); });
            var mapper = new Mapper(config);

            mappingService = new MappingService(mapper);
        }

        [Fact]
        public void MapToSegmentModelWhenJobProfileMessageSentThenItIsMappedCorrectly()
        {
            // Arrange
            var fullJPMessage = BuildJobProfileMessage();
            var message = JsonConvert.SerializeObject(fullJPMessage);
            var expectedResponse = BuildExpectedResponse();

            // Act
            var actualMappedModel = mappingService.MapToSegmentModel(message, SequenceNumber);

            // Assert
            expectedResponse.Should().BeEquivalentTo(actualMappedModel);
        }

        private JobProfileMessage BuildJobProfileMessage()
        {
            return new JobProfileMessage
            {
                JobProfileId = JobProfileId,
                CanonicalName = TestJobName,
                LastModified = lastModified,
                SOCLevelTwo = SocCodeId,
                CareerPathAndProgression = MarkupString,
            };
        }

        private CareerPathSegmentModel BuildExpectedResponse()
        {
            return new CareerPathSegmentModel
            {
                CanonicalName = TestJobName,
                SocLevelTwo = SocCodeId,
                Etag = null,
                DocumentId = JobProfileId,
                SequenceNumber = SequenceNumber,
                Data = new CareerPathSegmentDataModel
                {
                    LastReviewed = lastModified,
                    Markup = MarkupString,
                },
            };
        }
    }
}
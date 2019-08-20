using AutoMapper;
using DFC.App.CareerPath.AutoMapperProfiles;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DFC.App.CareerPath.IntegrationTests.ControllerTests
{
    [Trait("Integration Tests", "AutoMapper Tests")]
    public class AutoMapperProfileTests : IClassFixture<CustomWebApplicationFactory<DFC.App.CareerPath.Startup>>
    {

        private readonly CustomWebApplicationFactory<DFC.App.CareerPath.Startup> factory;

        public AutoMapperProfileTests(CustomWebApplicationFactory<DFC.App.CareerPath.Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public void AutoMapperProfileConfigurationForCareerPathSegmentModelProfileReturnSuccess()
        {
            // Arrange
            _ = factory.CreateClient();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid<CareerPathSegmentModelProfile>();

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void AutoMapperProfileConfigurationForAllProfilesReturnSuccess()
        {
            // Arrange
            _ = factory.CreateClient();
            var mapper = factory.Server.Host.Services.GetRequiredService<IMapper>();

            // Act
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            Assert.True(true);
        }
    }
}
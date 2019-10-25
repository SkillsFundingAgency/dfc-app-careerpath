using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.HomeControllerTests
{
    [Trait("Home Controller", "Error Tests")]
    public class HomeControllerErrorTests : BaseHomeController
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void HomeControllerErrorHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            var controller = BuildHomeController(mediaTypeName);

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            _ = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);

            controller.Dispose();
        }
    }
}

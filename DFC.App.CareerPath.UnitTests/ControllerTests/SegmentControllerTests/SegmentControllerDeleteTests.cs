﻿using DFC.App.CareerPath.Data.Models;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Xunit;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    [Trait("Segment Controller", "Delete Tests")]
    public class SegmentControllerDeleteTests : BaseSegmentController
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerDeleteReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            Guid documentId = Guid.NewGuid();
            var expectedResult = A.Fake<CareerPathSegmentModel>();
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Delete(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var okResult = Assert.IsType<OkResult>(result);

            A.Equals((int)HttpStatusCode.OK, okResult.StatusCode);

            controller.Dispose();
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public async void SegmentControllerDeleteReturnsNotFound(string mediaTypeName)
        {
            // Arrange
            Guid documentId = Guid.NewGuid();
            CareerPathSegmentModel expectedResult = null;
            var controller = BuildSegmentController(mediaTypeName);

            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).Returns(expectedResult);

            // Act
            var result = await controller.Delete(documentId).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeCareerPathSegmentService.GetByIdAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            var statusResult = Assert.IsType<NotFoundResult>(result);

            A.Equals((int)HttpStatusCode.NotFound, statusResult.StatusCode);

            controller.Dispose();
        }
    }
}
﻿using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Controllers;
using DFC.App.CareerPath.Data.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Net.Mime;

namespace DFC.App.CareerPath.UnitTests.ControllerTests.SegmentControllerTests
{
    public abstract class BaseSegmentController
    {
        protected BaseSegmentController()
        {
            FakeCareerPathSegmentService = A.Fake<ICareerPathSegmentService>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeLogService = A.Fake<ILogService>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new string[] { "*/*" },
            new string[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Application.Json },
        };

        protected ICareerPathSegmentService FakeCareerPathSegmentService { get; }

        protected ILogService FakeLogService { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected SegmentController BuildSegmentController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new SegmentController(FakeCareerPathSegmentService, FakeMapper, FakeLogService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}
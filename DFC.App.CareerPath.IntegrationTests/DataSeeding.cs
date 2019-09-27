﻿using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace DFC.App.CareerPath.IntegrationTests
{
    public static class DataSeeding
    {
        public const string DefaultArticleName = "segment-article";

        public static Guid DefaultArticleGuid => Guid.Parse("63DEA97E-B61C-4C14-15DC-1BD08EA20DC8");

        public static void SeedDefaultArticle(CustomWebApplicationFactory<Startup> factory)
        {
            const string url = "/segment";
            var models = new List<CareerPathSegmentModel>()
            {
                new CareerPathSegmentModel()
                {
                    DocumentId = DefaultArticleGuid,
                    CanonicalName = DefaultArticleName,
                    SocCodeTwo = "12345",
                    Data = new CareerPathSegmentDataModel
                    {
                        LastReviewed = DateTime.UtcNow,
                        Markup = "<p>some content</p>",
                    },
                },
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("C16B389D-91AD-4F3D-2485-9F7EE953AFE4"),
                    CanonicalName = $"{DefaultArticleName}-2",
                    SocCodeTwo = "12345",
                    Data = new CareerPathSegmentDataModel
                    {
                        LastReviewed = DateTime.UtcNow,
                        Markup = "<p>some content</p>",
                    },
                },
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("C0103C26-E7C9-4008-3F66-1B2DB192177E"),
                    CanonicalName = $"{DefaultArticleName}-3",
                    SocCodeTwo = "12345",
                    Data = new CareerPathSegmentDataModel
                    {
                        LastReviewed = DateTime.UtcNow,
                        Markup = "<p>some content</p>",
                    },
                },
            };

            var client = factory?.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            models.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()).GetAwaiter().GetResult());
        }
    }
}

using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;

namespace DFC.App.CareerPath.IntegrationTests.ControllerTests
{
    public static class DataSeeding
    {
        public static void SeedDefaultArticle(CustomWebApplicationFactory<DFC.App.CareerPath.Startup> factory, string article)
        {
            var careerPathSegmentModel = new List<CareerPathSegmentModel>()
            {
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("63DEA97E-B61C-4C14-85DC-1BD08EA20DC8"),
                    CanonicalName = article,
                    LastReviewed = DateTime.UtcNow,
                },
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("C16B389D-91AD-4F3D-B485-9F7EE953AFE4"),
                    CanonicalName = $"{article}-1",
                    LastReviewed = DateTime.UtcNow,
                },
                new CareerPathSegmentModel()
                {
                    DocumentId = Guid.Parse("C0103C26-E7C9-4008-BF66-1B2DB192177E"),
                    CanonicalName = $"{article}-2",
                    LastReviewed = DateTime.UtcNow,
                },
            };

            var client = factory?.CreateClient();

            client.DefaultRequestHeaders.Accept.Clear();

            //   careerPathSegmentModel.ForEach(f => client.PostAsync(url, f, new JsonMediaTypeFormatter()));
        }
    }
}
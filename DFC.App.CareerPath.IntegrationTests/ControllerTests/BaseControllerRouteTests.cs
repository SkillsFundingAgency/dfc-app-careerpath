using System;

namespace DFC.App.CareerPath.IntegrationTests.ControllerTests
{
    public abstract class BaseControllerRouteTests
    {
        protected const string DefaultArticleName = "segment-article";

        protected Guid DefaultArticleGuid => Guid.Parse("63DEA97E-B61C-4C14-15DC-1BD08EA20DC8");

        protected DateTime DefaultArticleCreated => new DateTime(2019, 09, 01, 12, 0, 0);
    }
}

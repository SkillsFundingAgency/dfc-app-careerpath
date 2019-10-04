using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using DFC.Functions.DI.Standard;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

[assembly: WebJobsStartup(typeof(DFC.App.CareerPath.MessageFunctionApp.Startup.WebJobsExtensionStartup), "Web Jobs Extension Startup")]

namespace DFC.App.CareerPath.MessageFunctionApp.Startup
{
    public class WebJobsExtensionStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(Environment.CurrentDirectory)
               .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
               .AddEnvironmentVariables()
               .Build();

            var segmentClientOptions = configuration.GetSection("CareerPathSegmentClientOptions").Get<SegmentClientOptions>();

            builder.AddDependencyInjection();

            builder.Services.AddSingleton<SegmentClientOptions>(segmentClientOptions);
            builder.Services.AddSingleton<HttpClient>(new HttpClient());
        }
    }
}

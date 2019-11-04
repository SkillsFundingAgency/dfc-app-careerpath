using AutoMapper;
using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: WebJobsStartup(typeof(DFC.App.CareerPath.MessageFunctionApp.Startup), "Web Jobs Extension Startup")]

namespace DFC.App.CareerPath.MessageFunctionApp
{
    public class Startup : IWebJobsStartup
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

            var services = builder?.Services;

            services.AddSingleton(segmentClientOptions);
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddTransient<IMessagePreProcessor, MessagePreProcessor>();
            services.AddTransient<IMessageProcessor, MessageProcessor>();
            services.AddTransient<ILogService, LogService>();
            services.AddScoped<ICorrelationIdProvider, RequestCorrelationIdProvider>();

            services.AddHttpClient(nameof(MessageProcessor), httpClient =>
            {
                httpClient.Timeout = segmentClientOptions.Timeout;
                httpClient.BaseAddress = segmentClientOptions.BaseAddress;
            });
        }
    }
}

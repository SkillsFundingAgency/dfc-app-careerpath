using AutoMapper;
using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Common.Services;
using DFC.App.CareerPath.MessageFunctionApp.HttpClientPolicies;
using DFC.App.CareerPath.MessageFunctionApp.Services;
using DFC.Functions.DI.Standard;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

[assembly: WebJobsStartup(typeof(DFC.App.CareerPath.MessageFunctionApp.Startup), "Web Jobs Extension Startup")]

namespace DFC.App.CareerPath.MessageFunctionApp
{
    [ExcludeFromCodeCoverage]
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
            services.AddScoped<IMessagePreProcessor, MessagePreProcessor>();
            services.AddScoped<IMessageProcessor, MessageProcessor>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ICorrelationIdProvider, InMemoryCorrelationIdProvider>();
            services.AddTransient(provider => new HttpClient());
            services.AddScoped<IHttpClientService, HttpClientService>();
            services.AddSingleton<IMappingService, MappingService>();
            services.AddSingleton<IMessagePropertiesService, MessagePropertiesService>();
        }
    }
}
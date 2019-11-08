﻿using AutoMapper;
using DFC.App.CareerPath.Common.Contracts;
using DFC.App.CareerPath.Common.Services;
using DFC.App.CareerPath.Data.Contracts;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.DraftSegmentService;
using DFC.App.CareerPath.Repository.CosmosDb;
using DFC.App.CareerPath.SegmentService;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DFC.App.CareerPath
{
    public class Startup
    {
        public const string ServiceBusOptionsAppSettings = "ServiceBusOptions";
        public const string CosmosDbConfigAppSettings = "Configuration:CosmosDbConnections:JobProfileSegment";

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var applicationInsightsServiceOptions = new ApplicationInsightsServiceOptions();
            applicationInsightsServiceOptions.InstrumentationKey = configuration["ApplicationInsights:InstrumentationKey"];
            services.AddApplicationInsightsTelemetry(applicationInsightsServiceOptions);

            var serviceBusOptions = configuration.GetSection(ServiceBusOptionsAppSettings).Get<ServiceBusOptions>();
            services.AddSingleton(serviceBusOptions ?? new ServiceBusOptions());

            var cosmosDbConnection = configuration.GetSection(CosmosDbConfigAppSettings).Get<CosmosDbConnection>();

            var documentClient = new DocumentClient(new Uri(cosmosDbConnection.EndpointUrl), cosmosDbConnection.AccessKey);

            services.AddSingleton(cosmosDbConnection);
            services.AddSingleton<IDocumentClient>(documentClient);
            services.AddSingleton<ICosmosRepository<CareerPathSegmentModel>, CosmosRepository<CareerPathSegmentModel>>();
            services.AddScoped<ICareerPathSegmentService, CareerPathSegmentService>();
            services.AddScoped<IDraftCareerPathSegmentService, DraftCareerPathSegmentService>();
            services.AddScoped<IJobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>, JobProfileSegmentRefreshService<RefreshJobProfileSegmentServiceBusModel>>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddScoped<ICorrelationIdProvider, RequestHeaderCorrelationIdProvider>();
            services.AddScoped<ILogService, LogService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Segment}/{action=Index}");
            });

            mapper?.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}

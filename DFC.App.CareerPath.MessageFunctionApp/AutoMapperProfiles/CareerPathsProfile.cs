﻿using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels.Save;

namespace DFC.App.JobProfileTasks.MessageFunctionApp.AutoMapperProfiles
{
    public class CareerPathsProfile : Profile
    {
        public CareerPathsProfile()
        {
            CreateMap<JobProfileServiceBusSaveModel, CareerPathSegmentModel>()
                .ForMember(d => d.DocumentId, s => s.MapFrom(a => a.JobProfileId))
                .ForMember(d => d.Data, s => s.MapFrom(a => a.Data));

            CreateMap<JobProfileCareerPathDataServiceBusModel, CareerPathSegmentDataModel>();
        }
    }
}
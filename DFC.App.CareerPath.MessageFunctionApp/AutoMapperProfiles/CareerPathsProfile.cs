using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;

namespace DFC.App.JobProfileTasks.MessageFunctionApp.AutoMapperProfiles
{
    public class CareerPathsProfile : Profile
    {
        public CareerPathsProfile()
        {
            CreateMap<JobProfileMessage, CareerPathSegmentModel>()
                .ForMember(d => d.DocumentId, s => s.MapFrom(a => a.JobProfileId))
                .ForPath(d => d.Data.LastReviewed, s => s.MapFrom(o => o.LastModified))
                .ForPath(d => d.Data.Markup, s => s.MapFrom(a => a.CareerPathAndProgression));
        }
    }
}
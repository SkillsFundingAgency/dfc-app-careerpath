using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Html;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    public class CareerPathSegmentDataModelProfile : Profile
    {
        public CareerPathSegmentDataModelProfile()
        {
            CreateMap<CareerPathSegmentDataModel, BodyDataViewModel>()
                ;
        }
    }
}

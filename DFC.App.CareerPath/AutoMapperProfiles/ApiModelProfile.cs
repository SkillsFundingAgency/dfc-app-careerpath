using AutoMapper;
using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    public class HtmlStringFormatter : IValueConverter<string, List<string>>
    {
        public List<string> Convert(string sourceMember, ResolutionContext context)
        {
            return new List<string> { sourceMember };
        }
    }

    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            CreateMap<CareerPathSegmentDataModel, CareerPathAndProgressionApiModel>()
                .ForMember(d => d.CareerPathAndProgression, opt => opt.ConvertUsing(new HtmlStringFormatter(), s => s.Markup))
                ;
        }
    }
}
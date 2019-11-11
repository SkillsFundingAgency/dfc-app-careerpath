using AutoMapper;
using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Data.Models;
using DFC.HtmlToDataTranslator.Services;
using System.Collections.Generic;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    public class HtmlStringFormatter : IValueConverter<string, List<string>>
    {
        public List<string> Convert(string sourceMember, ResolutionContext context)
        {
            var translator = new HtmlAgilityPackDataTranslator();
            var result = translator.Translate(sourceMember);
            return result;
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
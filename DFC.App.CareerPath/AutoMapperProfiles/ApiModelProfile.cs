using AutoMapper;
using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Data.Models;
using DFC.HtmlToDataTranslator.ValueConverters;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            CreateMap<CareerPathSegmentDataModel, CareerPathAndProgressionApiModel>()
                .ForMember(d => d.CareerPathAndProgression, opt => opt.ConvertUsing(new HtmlToStringValueConverter(), s => s.Markup))
                ;
        }
    }
}
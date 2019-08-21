using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Html;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    public class CareerPathSegmentModelProfile : Profile
    {
        public CareerPathSegmentModelProfile()
        {
            CreateMap<CareerPathSegmentModel, DocumentViewModel>()
                .ForMember(d => d.Content, s => s.MapFrom(a => new HtmlString(a.Content)))
                ;

            CreateMap<CareerPathSegmentModel, IndexDocumentViewModel>()
                ;
        }
    }
}

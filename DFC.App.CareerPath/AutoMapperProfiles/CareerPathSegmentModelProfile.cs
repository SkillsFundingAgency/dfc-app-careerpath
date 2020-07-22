using AutoMapper;
using DFC.App.CareerPath.Data.Models;
using DFC.App.CareerPath.Data.Models.ServiceBusModels;
using DFC.App.CareerPath.ViewModels;
using Microsoft.AspNetCore.Html;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class CareerPathSegmentModelProfile : Profile
    {
        public CareerPathSegmentModelProfile()
        {
            CreateMap<CareerPathSegmentModel, BodyViewModel>()
                .ForMember(d => d.Markup, s => s.MapFrom(a => new HtmlString(a.Data.Markup)));

            CreateMap<CareerPathSegmentModel, DocumentViewModel>()
                .ForMember(d => d.Markup, s => s.MapFrom(a => new HtmlString(a.Data.Markup)))
                .ForMember(d => d.LastReviewed, s => s.MapFrom(a => a.Data.LastReviewed));

            CreateMap<CareerPathSegmentModel, IndexDocumentViewModel>();

            CreateMap<CareerPathSegmentModel, RefreshJobProfileSegmentServiceBusModel>()
                .ForMember(d => d.JobProfileId, s => s.MapFrom(a => a.DocumentId))
                .ForMember(d => d.Segment, s => s.MapFrom(a => CareerPathSegmentDataModel.SegmentName));
        }
    }
}
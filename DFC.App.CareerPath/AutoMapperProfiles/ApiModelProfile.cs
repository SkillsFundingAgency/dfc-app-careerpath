﻿using AutoMapper;
using DFC.App.CareerPath.ApiModels;
using DFC.App.CareerPath.Data.Models;
using DFC.HtmlToDataTranslator.Services;
using DFC.HtmlToDataTranslator.ValueConverters;

namespace DFC.App.CareerPath.AutoMapperProfiles
{
    public class ApiModelProfile : Profile
    {
        public ApiModelProfile()
        {
            var htmlTranslator = new HtmlAgilityPackDataTranslator();
            var htmlToStringValueConverter = new HtmlToStringValueConverter(htmlTranslator);

            CreateMap<CareerPathSegmentDataModel, CareerPathAndProgressionApiModel>()
                .ForMember(d => d.CareerPathAndProgression, opt => opt.ConvertUsing(htmlToStringValueConverter, s => s.Markup))
                ;
        }
    }
}
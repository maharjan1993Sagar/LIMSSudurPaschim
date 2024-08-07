﻿using AutoMapper;
using LIMS.Domain.Common;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Models.Common;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class AddressAttributeValueProfile : Profile, IMapperProfile
    {
        public AddressAttributeValueProfile()
        {
            CreateMap<AddressAttributeValue, AddressAttributeValueModel>()
                .ForMember(dest => dest.Locales, mo => mo.Ignore());
            CreateMap<AddressAttributeValueModel, AddressAttributeValue>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.Locales, mo => mo.MapFrom(x => x.Locales.ToLocalizedProperty()));
        }

        public int Order => 0;
    }
}
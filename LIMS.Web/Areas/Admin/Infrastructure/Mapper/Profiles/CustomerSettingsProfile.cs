﻿using AutoMapper;
using LIMS.Domain.Customers;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Settings;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class CustomerSettingsProfile : Profile, IMapperProfile
    {
        public CustomerSettingsProfile()
        {
            CreateMap<CustomerSettings, CustomerUserSettingsModel.CustomerSettingsModel>()
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());
            CreateMap<CustomerUserSettingsModel.CustomerSettingsModel, CustomerSettings>()
                .ForMember(dest => dest.HashedPasswordFormat, mo => mo.Ignore())
                .ForMember(dest => dest.AvatarMaximumSizeBytes, mo => mo.Ignore())
                .ForMember(dest => dest.DownloadableProductsValidateUser, mo => mo.Ignore())
                .ForMember(dest => dest.OnlineCustomerMinutes, mo => mo.Ignore())
                .ForMember(dest => dest.SuffixDeletedCustomers, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}
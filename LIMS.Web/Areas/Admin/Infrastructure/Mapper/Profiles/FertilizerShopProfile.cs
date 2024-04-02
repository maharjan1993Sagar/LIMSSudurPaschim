using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Organizations;
using LIMS.Web.Areas.Admin.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class FertilizerShopProfile: Profile, IMapperProfile
    {
       
            public FertilizerShopProfile()
            {
                CreateMap<FertilizerShopModel, FertilizerShop>().ReverseMap();
            }


            public int Order => 0;
        
    }
}

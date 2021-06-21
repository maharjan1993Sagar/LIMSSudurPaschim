using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.DynamicMenu;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class SubSubMenuProfile: Profile, IMapperProfile
    {
        public SubSubMenuProfile()
        {
            CreateMap<SubSubMenu, SubSubMenuModel>().ReverseMap();
        }
        public int Order => 0; 
    }
  
}

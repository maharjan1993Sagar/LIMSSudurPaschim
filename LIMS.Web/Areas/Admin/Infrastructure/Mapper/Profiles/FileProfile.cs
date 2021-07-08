using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.DynamicMenu;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class FileProfile: Profile, IMapperProfile
    {
        public FileProfile()
        {
            CreateMap<NewsEventFile, NewsEventFileModel>().ReverseMap();
        }
        public int Order => 0; 
    }
  
}

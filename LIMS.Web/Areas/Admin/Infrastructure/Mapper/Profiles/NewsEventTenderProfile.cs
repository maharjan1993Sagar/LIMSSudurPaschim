using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class NewsEventTenderProfile: Profile, IMapperProfile
    {
        public NewsEventTenderProfile()
        {
            CreateMap<NewsEventTender, NewsEventTenderModel>().ReverseMap();
        }
        public int Order => 0; 
    }
  
}

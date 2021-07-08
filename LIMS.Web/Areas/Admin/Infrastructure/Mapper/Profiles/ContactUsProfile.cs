using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Breed;
using LIMS.Domain.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class ContactUsProfile: Profile, IMapperProfile
    {
        public ContactUsProfile()
        {
            CreateMap<ContactUs, ContactUsModel>().ReverseMap();

        }
        public int Order => 0; 
    }
  
}

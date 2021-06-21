using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Users;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class NlboUserProfile:Profile,IMapperProfile
    {
        public NlboUserProfile()
        {
            CreateMap<NlboUser, NlboUserModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}

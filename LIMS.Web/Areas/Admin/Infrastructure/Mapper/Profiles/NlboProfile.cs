using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.MoAMAC;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class NlboProfile:Profile,IMapperProfile
    {
        public NlboProfile()
        {
            CreateMap<Nlbo, NlboModel>().ReverseMap();

        }
        public int Order { get; set; } = 0;
    }
}

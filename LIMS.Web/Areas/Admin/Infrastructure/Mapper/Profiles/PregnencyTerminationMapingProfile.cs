using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Services;
using LIMS.Web.Areas.Admin.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class PregnencyTerminationMapingProfile: Profile, IMapperProfile
    {
        public PregnencyTerminationMapingProfile()
        {
            CreateMap<PregnencyTermination, PregnencyTerminationModel>();
        }
        public int Order => 0;
    }
}

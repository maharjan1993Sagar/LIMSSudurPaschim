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
    public class OtherOrganizationMapping:Profile,IMapperProfile
    {
        public OtherOrganizationMapping()
        {
            CreateMap<OtherOrganization, OtherOrganizationModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}

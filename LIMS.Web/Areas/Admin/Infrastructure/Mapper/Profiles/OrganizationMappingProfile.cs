using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LIMS.Domain.Organizations;
namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper
{
    public class OrganizationMappingProfile:Profile,IMapperProfile
    {
        public OrganizationMappingProfile()
        {
            CreateMap<Organization, OrganizationModel>().ReverseMap();
        }
        public int Order { get; set; } = 0;
    }
}

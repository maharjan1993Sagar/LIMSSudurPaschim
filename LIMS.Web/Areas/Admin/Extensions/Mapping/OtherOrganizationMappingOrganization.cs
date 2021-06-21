using LIMS.Domain.Organizations;
using LIMS.Web.Areas.Admin.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class OtherOrganizationMappingOrganization
    {
        public static  OtherOrganization ToEntity(this OtherOrganizationModel model )
        {
            return model.MapTo<OtherOrganizationModel,OtherOrganization>();
        }
        public static OtherOrganizationModel ToModel(this OtherOrganization entity)
        {
            return entity.MapTo<OtherOrganization,OtherOrganizationModel>();
        }
        public static OtherOrganization ToEntity(this OtherOrganizationModel source, OtherOrganization destination)
        {
            return source.MapTo(destination);
        }
    }
}

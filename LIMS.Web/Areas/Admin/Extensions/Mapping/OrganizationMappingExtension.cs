using LIMS.Domain.Organizations;
using LIMS.Web.Areas.Admin.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class OrganizationMappingExtension
    {
        public static OrganizationModel ToModel(this Organization entity)
        {
            return entity.MapTo<Organization, OrganizationModel>();
        }

        public static Organization ToEntity(this OrganizationModel model)
        {
            return model.MapTo<OrganizationModel, Organization>();
        }

        public static Organization ToEntity(this OrganizationModel model, Organization destination)
        {
            return model.MapTo(destination);
        }
    }
}

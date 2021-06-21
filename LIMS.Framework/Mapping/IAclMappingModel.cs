using LIMS.Core.ModelBinding;
using LIMS.Framework.Mvc.Models;
using System.Collections.Generic;

namespace LIMS.Framework.Mapping
{
    public interface IAclMappingModel
    {
        [LIMSResourceDisplayName("Admin.Catalog.Categories.Fields.SubjectToAcl")]
        bool SubjectToAcl { get; set; }
        [LIMSResourceDisplayName("Admin.Catalog.Categories.Fields.AclCustomerRoles")]
        List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        string[] SelectedCustomerRoleIds { get; set; }
    }
}

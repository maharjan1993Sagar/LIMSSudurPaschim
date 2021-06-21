using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public class CustomerRolePermissionModel : BaseEntityModel
    {
        public CustomerRolePermissionModel()
        {
            Actions = new List<string>();
        }
        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Acl.Fields.Name")]
        public string Name { get; set; }

        public string SystemName { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Acl.Fields.Access")]
        public bool Access { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Acl.Fields.Actions")]
        public IList<string> Actions { get; set; }
    }
}

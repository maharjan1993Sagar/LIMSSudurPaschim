using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Customers
{
    public partial class CustomerRoleModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Name")]

        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.FreeShipping")]

        public bool FreeShipping { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.TaxExempt")]
        public bool TaxExempt { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.Active")]
        public bool Active { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.IsSystemRole")]
        public bool IsSystemRole { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.SystemName")]
        public string SystemName { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.EnablePasswordLifetime")]
        public bool EnablePasswordLifetime { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.MinOrderAmount")]
        [UIHint("DecimalNullable")]
        public decimal? MinOrderAmount { get; set; }

        [LIMSResourceDisplayName("Admin.Customers.CustomerRoles.Fields.MaxOrderAmount")]
        [UIHint("DecimalNullable")]
        public decimal? MaxOrderAmount { get; set; }

    }
}
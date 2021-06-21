using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Affiliates
{
    public partial class AffiliateListModel : BaseModel
    {
        [LIMSResourceDisplayName("Admin.Affiliates.List.SearchFirstName")]
        
        public string SearchFirstName { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.List.SearchLastName")]
        
        public string SearchLastName { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.List.SearchFriendlyUrlName")]
        
        public string SearchFriendlyUrlName { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.List.LoadOnlyWithOrders")]
        public bool LoadOnlyWithOrders { get; set; }
        [LIMSResourceDisplayName("Admin.Affiliates.List.OrdersCreatedFromUtc")]
        [UIHint("DateNullable")]
        public DateTime? OrdersCreatedFromUtc { get; set; }
        [LIMSResourceDisplayName("Admin.Affiliates.List.OrdersCreatedToUtc")]
        [UIHint("DateNullable")]
        public DateTime? OrdersCreatedToUtc { get; set; }
    }
}
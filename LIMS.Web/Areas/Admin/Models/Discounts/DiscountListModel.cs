using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Discounts
{
    public partial class DiscountListModel : BaseModel
    {
        public DiscountListModel()
        {
            AvailableDiscountTypes = new List<SelectListItem>();
        }

        [LIMSResourceDisplayName("Admin.Promotions.Discounts.List.SearchDiscountCouponCode")]
        
        public string SearchDiscountCouponCode { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Discounts.List.SearchDiscountName")]
        
        public string SearchDiscountName { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.Discounts.List.SearchDiscountType")]
        public int SearchDiscountTypeId { get; set; }
        public IList<SelectListItem> AvailableDiscountTypes { get; set; }
    }
}
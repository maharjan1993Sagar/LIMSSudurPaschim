using System;
using LIMS.Web.Areas.Admin.Models.Common;
using LIMS.Core.Models;
using LIMS.Core.ModelBinding;

namespace LIMS.Web.Areas.Admin.Models.Affiliates
{
    public partial class AffiliateModel : BaseEntityModel
    {
        public AffiliateModel()
        {
            Address = new AddressModel();
        }

        [LIMSResourceDisplayName("Admin.Affiliates.Fields.ID")]
        public override string Id { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.Fields.URL")]
        public string Url { get; set; }


        [LIMSResourceDisplayName("Admin.Affiliates.Fields.AdminComment")]
        
        public string AdminComment { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.Fields.FriendlyUrlName")]
        
        public string FriendlyUrlName { get; set; }
        
        [LIMSResourceDisplayName("Admin.Affiliates.Fields.Active")]
        public bool Active { get; set; }

        public AddressModel Address { get; set; }

        #region Nested classes
        
        public partial class AffiliatedOrderModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Affiliates.Orders.Order")]
            public override string Id { get; set; }
            public int OrderNumber { get; set; }

            public string OrderCode { get; set; }

            [LIMSResourceDisplayName("Admin.Affiliates.Orders.OrderStatus")]
            public string OrderStatus { get; set; }

            [LIMSResourceDisplayName("Admin.Affiliates.Orders.PaymentStatus")]
            public string PaymentStatus { get; set; }

            [LIMSResourceDisplayName("Admin.Affiliates.Orders.ShippingStatus")]
            public string ShippingStatus { get; set; }

            [LIMSResourceDisplayName("Admin.Affiliates.Orders.OrderTotal")]
            public string OrderTotal { get; set; }

            [LIMSResourceDisplayName("Admin.Affiliates.Orders.CreatedOn")]
            public DateTime CreatedOn { get; set; }
        }

        public partial class AffiliatedCustomerModel : BaseEntityModel
        {
            [LIMSResourceDisplayName("Admin.Affiliates.Customers.Name")]
            public string Name { get; set; }
        }

        #endregion
    }
}
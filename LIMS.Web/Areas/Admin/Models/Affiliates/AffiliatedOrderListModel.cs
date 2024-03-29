﻿using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Affiliates
{
    public partial class AffiliatedOrderListModel : BaseModel
    {
        public AffiliatedOrderListModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
            AvailableShippingStatuses = new List<SelectListItem>();
        }

        public string AffliateId { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.Orders.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.Orders.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [LIMSResourceDisplayName("Admin.Affiliates.Orders.OrderStatus")]
        public int OrderStatusId { get; set; }
        [LIMSResourceDisplayName("Admin.Affiliates.Orders.PaymentStatus")]
        public int PaymentStatusId { get; set; }
        [LIMSResourceDisplayName("Admin.Affiliates.Orders.ShippingStatus")]
        public int ShippingStatusId { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }
        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }
        public IList<SelectListItem> AvailableShippingStatuses { get; set; }
    }
}
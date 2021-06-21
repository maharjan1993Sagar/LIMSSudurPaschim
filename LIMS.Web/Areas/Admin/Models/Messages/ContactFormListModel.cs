﻿using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class ContactFormListModel : BaseModel
    {
        public ContactFormListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }
        [LIMSResourceDisplayName("Admin.System.ContactForm.List.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }

        [LIMSResourceDisplayName("Admin.System.ContactForm.List.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }

        [LIMSResourceDisplayName("Admin.System.ContactForm.List.Email")]
        
        public string SearchEmail { get; set; }

        [LIMSResourceDisplayName("Admin.System.ContactForm.List.Store")]
        public string StoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

    }
}
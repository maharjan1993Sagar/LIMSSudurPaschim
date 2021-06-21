using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class MessageTemplateListModel : BaseModel
    {
        public MessageTemplateListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }
        [LIMSResourceDisplayName("Admin.ContentManagement.MessageTemplates.List.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Admin.ContentManagement.MessageTemplates.List.SearchStore")]
        public string SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
    }
}
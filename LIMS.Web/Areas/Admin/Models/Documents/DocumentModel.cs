using LIMS.Framework.Mapping;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Models.Documents
{
    public class DocumentModel : BaseEntityModel, IAclMappingModel, IStoreMappingModel
    {
        public DocumentModel()
        {
            AvailableDocumentTypes = new List<SelectListItem>();
        }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Number")]
        public string Number { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Description")]
        public string Description { get; set; }

        public string ParentDocumentId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Picture")]
        [UIHint("Picture")]
        public string PictureId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Download")]
        [UIHint("Download")]
        public string DownloadId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Published")]
        public bool Published { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Flag")]
        public string Flag { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Link")]
        public string Link { get; set; }

        public string CustomerId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Status")]
        public int StatusId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Reference")]
        public int ReferenceId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Object")]
        public string ObjectId { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.DocumentType")]
        public string DocumentTypeId { get; set; }
        public IList<SelectListItem> AvailableDocumentTypes { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.CustomerEmail")]
        public string CustomerEmail { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Username")]
        public string Username { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.CurrencyCode")]
        public string CurrencyCode { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.TotalAmount")]
        public decimal TotalAmount { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.OutstandAmount")]
        public decimal OutstandAmount { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.Quantity")]
        public int Quantity { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.DocDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DocDate { get; set; }

        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.DueDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DueDate { get; set; }

        //ACL
        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.SubjectToAcl")]
        public bool SubjectToAcl { get; set; }
        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.AclCustomerRoles")]
        public List<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public string[] SelectedCustomerRoleIds { get; set; }

        //Store mapping
        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [LIMSResourceDisplayName("Admin.Documents.Document.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public string[] SelectedStoreIds { get; set; }
    }
}

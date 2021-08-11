using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.GeneralCMS

{
   public class ImportantLinksModel:BaseEntity
    {
       [LIMSResourceDisplayName("Admin.ImportantLinks.LinkName")]
        public string LinkName { get; set; }
        [LIMSResourceDisplayName("Admin.ImportantLinks.SerialNo")]
        public int SerialNo { get; set; }
        [LIMSResourceDisplayName("Admin.ImportantLinks.LinkNameNepali")]
        public string LinkNameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.ImportantLinks.URL")]
        public string URL { get; set; }  
      
    }
}

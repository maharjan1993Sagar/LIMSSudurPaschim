using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.GeneralCMS

{
   public class ImportantLinks:BaseEntity
    {
        public ImportantLinks()
        {
            this.ImportantLinkId = Guid.NewGuid();
        }
        public Guid ImportantLinkId { get; set; }
        public string LinkName { get; set; }
        public string LinkNameNepali { get; set; }

        public int SerialNo { get; set; }
        public string URL { get; set; }       
        public string UserId {get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

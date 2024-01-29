using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.BasicSetup
{
  public class Category:BaseEntity
    {
        public Category()
        {
            this.CategoryId = Guid.NewGuid();
        }
        public Guid CategoryId { get; set; }
        public string NameEnglish { get; set; }
        public string NameNepali { get; set; }
        public string Type { get; set; }
        //public string UnitShortName { get; set; }
        //public string Description { get; set; }
    }
}

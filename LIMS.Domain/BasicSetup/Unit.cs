using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.BasicSetup
{
  public  class Unit:BaseEntity
    {
        public Unit()
        {
            this.UnitId = Guid.NewGuid();
        }
        public Guid UnitId { get; set; }
        public string UnitNameEnglish { get; set; }
        public string UnitNameNepali { get; set; }

        public string UnitShortName { get; set; }
        public string Description { get; set; }
    }
}

using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Activities
{
    public class Activity:BaseEntity
    {
        public Activity()
        {
            this.ActivityId = Guid.NewGuid();
        }
        public Guid ActivityId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityNameNepali { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string AdminId { get; set; }
    }
}

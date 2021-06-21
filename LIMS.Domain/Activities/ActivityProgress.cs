using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Activities
{
   public class ActivityProgress:BaseEntity
    {
        public ActivityProgress()
        {
            this.ActivityProgressId = Guid.NewGuid();
            this.Activity = new Activity();
            this.FiscalYear = new FiscalYear();
            this.Unit = new Unit();
        }
        public Guid ActivityProgressId { get; set; }
        public Activity Activity { get; set; }
        public string ActivityId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public Unit Unit { get; set; }
        public string UnitId { get; set; }
        public string Month { get; set; }
        public string Quater { get; set; }
        public string Progress { get; set; }
        public string CreatedBy { get; set; }
        public string AdminId { get; set; }

    }
}

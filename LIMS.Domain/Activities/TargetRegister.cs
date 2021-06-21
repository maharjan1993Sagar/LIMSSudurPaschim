using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Activities
{
    public class TargetRegister:BaseEntity
    {
        public TargetRegister()
        {
            this.TargetId = Guid.NewGuid();
            this.Activity = new Activity();
            this.Unit = new Unit();
            this.FiscalYear = new FiscalYear();

      }
        public Guid TargetId { get; set; }
        public Activity Activity { get; set; }
        public string ActivityId { get; set; }
        public Unit Unit { get; set; }
        public string UnitId { get; set; }
        public string QuaterOneTarget { get; set; }
        public string QuaterTwoTarget { get; set; }
        public string QuaterThreeTarget { get; set; }
        public string QuaterFourTarget { get; set; }
        public string AnualTarget { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string CreatedBy { get; set; }
        public string AdminId { get; set; }
    }
}

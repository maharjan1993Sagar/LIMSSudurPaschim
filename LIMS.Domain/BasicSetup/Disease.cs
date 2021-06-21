using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.BasicSetup
{
   public class Disease:BaseEntity
    {
        public Disease()
        {
            this.DiseaseId = Guid.NewGuid();
        }
        public Guid DiseaseId { get; set; }
        public string DiseaseNameNepali { get; set; }
        public string DiseaseNameEnglish { get; set; }
        public string Symptoms { get; set; }
        public string ShortName { get; set; }
    }
}

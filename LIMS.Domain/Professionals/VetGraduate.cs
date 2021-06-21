using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Professionals
{
  public  class VetGraduate:BaseEntity
    {
        public VetGraduate()
        {
            this.VetGraduateId = Guid.NewGuid();
        }
        public Guid VetGraduateId { get; set; }
        public string NameEnglish { get; set; }
        public string NameNepali { get; set; }
        public string Descriptions { get; set; }
    }
}

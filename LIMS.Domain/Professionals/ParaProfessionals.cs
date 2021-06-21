using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Professionals
{
  public  class ParaProfessionals:BaseEntity
    {
        public ParaProfessionals()
        {
            this.ParaProfessionalsId = Guid.NewGuid();
        }
        public Guid ParaProfessionalsId { get; set; }
        public string NameEnglish { get; set; }
        public string NameNepali { get; set; }

        public string Descriptions { get; set; }
    }
}

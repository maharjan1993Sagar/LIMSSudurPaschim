using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AnimalHealth
{
    public class Sample:BaseEntity
    {
        public string SampleBoxNo { get; set; }

        public string LabrotoryName { get; set; }

        public string SampleType { get; set; }
        public string CreatedBy { get; set; }
    }
}

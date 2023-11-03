using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.LocalStructure
{
    public class LocalLevels:BaseEntity
    {
        public int ProvinceCode { get; set; }
        public string Province { get; set; }
        public int DistrictCode { get; set; }
        public string District { get; set; }
        public string DistrictNepali { get; set; }
        public int MunicipalCode { get; set; }
        public string Municipality { get; set; }

    }
}

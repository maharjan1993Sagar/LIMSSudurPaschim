using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.LocalStructure
{
    public class LocalLevels:BaseEntity
    {
        public string ProvinceCode { get; set; }
        public string Province { get; set; }
        public string DistrictCode { get; set; }
        public string District { get; set; }
        public string MunicipalCode { get; set; }
        public string Municipality { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.LocalStructure
{
    public class LocalDto
    {
        public string Province { get; set; }
        public List<Districts> Districts { get; set; }
    }
    public class Districts
    {
        public string District { get; set; }
        public List<string> LocalLevel { get; set; }
    }
   

}

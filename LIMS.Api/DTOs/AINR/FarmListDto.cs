using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AINR
{
    public class FarmListDto: BaseApiEntityModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}

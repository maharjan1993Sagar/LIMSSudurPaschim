using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AINR
{
    public class SpeciesDto: BaseApiEntityModel
    {
        public string NepaliName { get; set; }

        public string EnglishName { get; set; }
 
    }
}

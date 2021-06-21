using LIMS.Api.Models;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.RationBalance
{
    public class AnimalFeedDto: BaseApiEntityModel
    {
      
        public Species species { get; set; }
        public string age { get; set; }
        public string Dm { get; set; }
        public string Tdn { get; set; }
        public string Cp { get; set; }
        public string Dcp { get; set; }
        public string P { get; set; }
        public string CA { get; set; }
        public string VitaminA { get; set; }
        public string Remarks { get; set; }

    }
}

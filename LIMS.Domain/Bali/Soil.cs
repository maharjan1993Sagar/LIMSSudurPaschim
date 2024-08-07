﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class Soil:BaseEntity
    {
        public string SampleNo { get; set; }
        public string FarmerName { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string SoilStructure { get; set; }
        public string Ph { get; set; }
        public string PrangricPadartha { get; set; }
        public string Nitrogen { get; set; }
        public string Phosporus { get; set; }
        public string Potas { get; set; }
        public string phoneNo { get; set; }
        public string Zinc { get; set; }
        public string Boron { get; set; }
        public string Othermicronutrients { get; set; }
        public string Unit { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
    }
}

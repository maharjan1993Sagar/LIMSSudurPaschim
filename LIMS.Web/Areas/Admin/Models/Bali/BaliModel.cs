﻿using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class BaliModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.Bali.CropCategory")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Lims.Bali.CropName")]

        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Lims.Bali.Area")]
        [UIHint("nullabledecimal")]

        public string Area { get; set; }
        [LIMSResourceDisplayName("Lims.Bali.Productivity")]
        [UIHint("nullabledecimal")]

        public string Productivity { get; set; }
        [LIMSResourceDisplayName("Lims.Bali.Production")]

        public string Production { get; set; }
        [LIMSResourceDisplayName("Lims.Bali.FiscalYear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Lims.Bali.Unit")]

        public string UnitId { get; set; }


    }
}

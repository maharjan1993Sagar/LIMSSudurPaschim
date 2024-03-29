﻿using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.StatisticalData
{
    public class ServicesModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.SpeciesName")]
        public string SpeciesName { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.BreedName")]

        public string BreedName { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.ServiceType")]

        public string ServicesType { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.Quantity")]

        public string Quantity { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Unit")]

        public string Unit { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYear")]

        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Provience")]
        public string Provience { get; set; }

        [LIMSResourceDisplayName("Admin.Common.District")]
        public string District { get; set; }

        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]
        public string LocalLevel { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Ward")]
        public string Ward { get; set; }

        [LIMSResourceDisplayName("Admin.Common.Tole")]
        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.Date")]
        public string Date { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.Quater")]

        public string Quater { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.Farm")]

        public string FarmId { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.Vaccination")]

        public string Vaccination { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.AnimalHealth")]

        public string AnimalHealthService { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.TreatMentType")]

        public string TreatmentType { get; set; }
        [LIMSResourceDisplayName("Admin.StatisticalData.Service.Month")]

        public string Month { get; set; }

    }
}

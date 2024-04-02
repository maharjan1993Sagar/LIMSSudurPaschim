using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class FertilizerDistributionModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.Distributor")]

        public string Distributor { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.FertilizerType")]

        public string FertilizerType { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.NepaliDate")]

        public string NepaliDate { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.EnglishDate")]

        public DateTime EnglishDate { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.Quantity")]

        public decimal Quantity { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.UnitId")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.FarmerId")]

        public string FarmerId { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.FarmerName")]

        public string FarmerName { get; set; }
        [LIMSResourceDisplayName("Admin.Common.PhoneNo")]

        public string PhoneNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.CitizenshipNo")]

        public string CitizenshipNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.IssuedDate")]

        public string IssuedDate { get; set; }
        [LIMSResourceDisplayName("Admin.Common.IssuedDistrict")]

        public string IssuedDistrict { get; set; }
        [LIMSResourceDisplayName("Admin.Common.FiscalYearId")]


        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.WardNo")]

        public string WardNo { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("Admin.FertilizerDistribution.Remarks")]

        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class FertilizerDistribution:BaseEntity
    {
        public string Distributor { get; set; }
        public string FertilizerType { get; set; }
        public string NepaliDate { get; set; }
        public DateTime EnglishDate { get; set; }
        public decimal Quantity { get; set; }
        public string UnitId { get; set; }
        public Unit Unit { get; set; }
        public string FarmerId { get; set; }
        public Farmer Farmer { get; set; }
        public string FarmerName { get; set; }
        public string PhoneNo { get; set; }
        public string CitizenshipNo { get; set; }
        public string IssuedDate { get; set; }
        public string IssuedDistrict { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Organizations;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.VaccinationInventory
{
    public class DistributedVaccine:BaseEntity
    {
        public DistributedVaccine()
        {
            this.Vaccination = new VaccinationType();
            this.FiscalYear = new FiscalYear();
            this.Unit = new Unit();
        }
        public VaccinationType Vaccination { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public Unit Unit { get; set; }
        public string FiscalYearId { get; set; }
        public string UnitId { get; set; }
        public string VaccinationTypeId { get; set; }
        public string Quantity { get; set; }
        public string Propose { get; set; }
        public string ReceivedBy { get; set; }
        public string Amount { get; set; }
        public string ReceivedFrom { get; set; }
        public string Date { get; set; }
        public Organization Organization { get; set; }
        public string OrganizationId { get; set; }

        public string Month { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}

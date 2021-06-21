using LIMS.Domain.AInR;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.SemenDistribution
{
   public  class SemenDistribution:BaseEntity
    {
        public SemenDistribution()
        {
            this.SemenId = Guid.NewGuid();
            this.ServiceProvider = new ServiceProvider();
        }
        public AnimalRegistration AnimalRegistration { get; set; }
        public string AnimalRegistrationId { get; set; }

        public Guid SemenId { get; set; }
        public ServiceProvider ServiceProvider { get; set; }
        public string ServiceProviderId { get; set; }
        public string SemenBullNo { get; set; }
        public string Dose { get; set; }
        public string TotalAmount { get; set; }
        public string Date { get; set; }
        public string FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }
        public string OrganizationName { get; set; }
    }
}

using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Bali
{
    public class Anugaman:BaseEntity
    {
        public Category Category { get; set; }
        public string CategoryId { get; set; }
        public string ObservedBy { get; set; } //which item is selected
        public string FarmerId { get; set; } //Entity Id
        public string Name { get; set; } //Entity Id
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }
        public string Type { get; set; } //Training, Subsidy
        public string NepaliDate { get; set; }
        public DateTime EnglishDate { get; set; }
        public string Month { get; set; }
        public string Observations { get; set; }
        public string Suggestion { get; set; }
        public string Remarks { get; set; }
        public IList<string> ImageUrls { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public string PhoneNumber { get; set; }
        public string VerifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

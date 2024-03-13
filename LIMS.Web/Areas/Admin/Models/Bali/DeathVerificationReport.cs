using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{ 
    public class VerificationHeader
{
        public string Level { get; set; }
        public string LocalLevel { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string LogoUrlRight { get; set; }
        public string LogoUrlLeft { get; set; }
        public string Department { get; set; }
        public string PaSa { get; set; }
        public string ChaNa { get; set; }
        public string Today { get; set; }
    }

    public class DeathVerificationReportModel
    {
        public VerificationHeader VerificationHeader { get; set; }
        public DeathVerification DeathVerification { get; set; }

    }
}

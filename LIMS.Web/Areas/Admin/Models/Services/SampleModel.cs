using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class SampleModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Sample.SampleBoxNo")]
        public string SampleBoxNo { get; set; }
        [LIMSResourceDisplayName("Admin.Sample.LabrotoryName")]


        public string LabrotoryName { get; set; }
        [LIMSResourceDisplayName("Admin.Sample.SampleType")]

        public string SampleType { get; set; }
        [LIMSResourceDisplayName("Admin.Sample.TestingCharge")]

        public string TestingCharge { get; set; }
        [LIMSResourceDisplayName("Admin.Sample.ReceiptNo")]

        public string ReceiptNo { get; set; }
        [LIMSResourceDisplayName("Admin.Sample.Result")]
        public string Result { get; set; }
    }
}

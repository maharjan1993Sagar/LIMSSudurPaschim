using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class MainActivityCodeModel:BaseEntity
    {
        [LIMSResourceDisplayName("Lims.MainActivityCode.Limbis_code")]
        public string Limbis_Code { get; set; }

    }
}

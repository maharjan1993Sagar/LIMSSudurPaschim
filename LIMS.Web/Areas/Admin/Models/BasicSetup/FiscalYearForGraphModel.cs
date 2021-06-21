using LIMS.Core.Models;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.BasicSetup
{
    public class FiscalYearForGraphModel:BaseEntity
    {
        public List<string> FiscalYear { get; set; }
    }
}

using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Vaccination
{
    public class ServiceRowData
    {
        public string Id { get; set; }
        public string SpeciesId { get; set; }
        public string SpeciesName { get; set; }
        public string Quantity{ get; set; }
    }
}

using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Country
{
    public partial class CountriesListModel : BaseModel
    {
        public CountriesListModel() { }

        [LIMSResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
        public string CountryName { get; set; }

    }
}

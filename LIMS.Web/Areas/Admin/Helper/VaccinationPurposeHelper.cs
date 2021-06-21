using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class VaccinationPurposeHelper
    {
        public static List<SelectListItem> GetVaccinationPurpose()
        {
            return new List<SelectListItem>() {
                new SelectListItem{
                    Text="Purna khop",
                    Value="Purna khop"
                },
                new SelectListItem{
                    Text="Mahamari",
                    Value="Mahamari"
                },
                new SelectListItem{
                    Text="Niyamit khop",
                    Value="Niyamit khop"
                },
                 new SelectListItem{
                    Text="Sarkar le taya garya karyakram",
                    Value="Sarkar le taya garya karyakram"
                },

            };
        }
    }
}

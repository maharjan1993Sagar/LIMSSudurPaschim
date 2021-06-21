using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class VaccinationReceivedHelper
    {
        public static List<SelectListItem> GetVaccinationReceived()
        {
            return new List<SelectListItem>() {
                new SelectListItem{
                    Text="Funded",
                    Value="Funded"
                },
                new SelectListItem{
                    Text="Bought",
                    Value="Bought"
                },
                

            };
        }

    }
}

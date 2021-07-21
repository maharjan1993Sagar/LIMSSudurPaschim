using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;


namespace LIMS.Website1.Data
{
   public class GetResource:IGetResource
    {
        public string GetByName(string name)
        { 
            string str = Resources.Resource.ResourceManager.GetString(name, System.Globalization.CultureInfo.CurrentCulture);

            return str;
        }
    }
}

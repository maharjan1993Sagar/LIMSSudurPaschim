using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public class NumberHelper
    {
        public static string EnglishToNepaliNumber(string input)
        {

            return input.Replace('०', '0')
                    .Replace('१', '1')
                    .Replace('२', '2')
                    .Replace('३', '3')
                    .Replace('४', '4')
                    .Replace('५', '5')
                    .Replace('६', '6')
                    .Replace('७', '7')
                    .Replace('८', '8')
                    .Replace('९', '9');
        }
    }
}

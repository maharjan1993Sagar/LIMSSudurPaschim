using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class NoticeType
    {
        public static string Notice = "Notice";
        public static string News = "News";
        public static string Event = "Event";
        public static string Tender = "Tender";
        public static string Publication = "Publication";
        public static List<string> GetTypes()
        {
            var types = new List<string>();
            types.Add(Notice);
            types.Add(News);
            types.Add(Event);
            types.Add(Tender);
            types.Add(Publication);
            return types;
        }
    }
}

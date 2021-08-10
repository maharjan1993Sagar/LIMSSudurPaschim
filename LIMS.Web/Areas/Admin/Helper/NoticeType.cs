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
        public static string Rules = "Rules & Regulation";
        public static string Directives = "Directives";
        public static string Reports = "Reports";
        public static string Act = "Act & Policies";
        public static string OtherFiles = "OtherFiles";
       public static string PressRelease = "PressRelease";
      public static string Letter = "Letter";
        public static List<string> GetTypes()
        {
            var types = new List<string>();
            types.Add(Notice);
            types.Add(News);
            types.Add(Event);
            types.Add(Tender);
            types.Add(Publication);
            types.Add(Rules);
            types.Add(Directives);
            types.Add(Reports);
            types.Add(Act);
            types.Add(PressRelease);
            types.Add(Letter);
            types.Add(OtherFiles);
            return types;
        }
    }
}

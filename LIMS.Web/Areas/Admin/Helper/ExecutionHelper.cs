using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    
    public static class ExecutionHelper
    {
        public static string LocalLevel = "Tokha Municipality";
        public static List<SelectListItem> GetTypeOfExecution()
        {
            return new List<SelectListItem> {
        new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="सोझै खरिद",
                    Value="1"
                },
                 new SelectListItem
                 {
                     Text = "परामर्श मार्फत",
                     Value = "2"
                 },

                    new SelectListItem
                  {
                      Text = "उपभोक्ता समिति",
                      Value = "3"
                  },
                       new SelectListItem
                  {
                      Text = "बोलपत्र मार्फत",
                      Value = "4"
                  },



            };
        }
        public static List<SelectListItem> GetPlanningTypes()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="योजना",
                    Value="1"
                },
                 new SelectListItem {
                    Text="कार्यक्रम",
                    Value="2"
                },
            };
        }
        public static List<SelectListItem> GetExecTypes()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="Training",
                    Value="Training"
                },
                 new SelectListItem {
                    Text="Subsidy",
                    Value="Subsidy"
                },
                  new SelectListItem {
                    Text="Input Supply",
                    Value="Input Supply"
                },
                   new SelectListItem {
                    Text="Other",
                    Value="Other"
                }
            };
        }
        public static List<SelectListItem> GetXetras()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="कृषि विकास",
                    Value="कृषि विकास"
                },
                 new SelectListItem {
                    Text="पशु तथा मत्स्य विकास ",
                    Value="पशु तथा मत्स्य विकास "
                },
            };
        }
            
        public static List<SelectListItem> GetUpaXetras()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="कृषि",
                    Value="कृषि"
                },
                 new SelectListItem {
                    Text="सहकारी तथा वित्तीय क्षेत्र",
                    Value="सहकारी तथा वित्तीय क्षेत्र"
                },
            };
        }
        public static List<SelectListItem> GetPrakar()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="चालु खर्च",
                    Value="1"
                },
                 new SelectListItem {
                    Text="पुँजीगत खर्च",
                    Value="2"
                },
            };
        }
        public static List<SelectListItem> GetKista()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="पहिलो किस्ता",
                    Value="पहिलो किस्ता"
                },
                 new SelectListItem {
                    Text="दोस्रो किस्ता",
                    Value="दोस्रो किस्ता"
                },
                 new SelectListItem {
                    Text="आन्तिम किस्ता",
                    Value="आन्तिम किस्ता"
                },
                 new SelectListItem {
                    Text="एकमुस्ट भुक्तानी",
                    Value="एकमुस्ट भुक्तानी"
                },
            };
        }
        public static List<SelectListItem> BiniyojanType()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="नगर स्तरिय बजेट",
                    Value="नगरस्तरिय"
                },
                 new SelectListItem {
                    Text="वडा स्तरिय बजेट",
                    Value="वडास्तरिय"
                },
                  new SelectListItem {
                    Text="नगर प्रमुखको तोक आदेस",
                    Value="नगर प्रमुखको तोक आदेस"
                },
                   new SelectListItem {
                    Text="नगर उपप्रमुखको तोक आदेस",
                    Value="नगर उपप्रमुखको तोक आदेस"
                },
                     new SelectListItem {
                    Text="नगर कार्यपालिकाको निर्णय",
                    Value="नगर कार्यपालिकाको निर्णय"
                },
                       new SelectListItem {
                    Text="प्र. प्र. अ. को तोक आदेस",
                    Value="प्र. प्र. अ. को तोक आदेस"
                },  new SelectListItem {
                    Text="कार्यपालिकाको निर्णय",
                    Value="कार्यपालिकाको निर्णय"
                },
                         new SelectListItem {
                    Text="नगर सभाको निर्णय",
                    Value="नगर सभाको निर्णय"
                },

            };
        }
        public static List<SelectListItem> AdeshType()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },

                  new SelectListItem {
                    Text="नगर प्रमुखको तोक आदेस",
                    Value="नगर प्रमुखको तोक आदेस"
                },
                   new SelectListItem {
                    Text="नगर उपप्रमुखको तोक आदेस",
                    Value="नगर उपप्रमुखको तोक आदेस"
                },
                     new SelectListItem {
                    Text="नगर कार्यपालिकाको निर्णय",
                    Value="नगर कार्यपालिकाको निर्णय"
                },
                       new SelectListItem {
                    Text="प्र. प्र. अ. को तोक आदेस",
                    Value="प्र. प्र. अ. को तोक आदेस"
                },  new SelectListItem {
                    Text="कार्यपालिकाको निर्णय",
                    Value="कार्यपालिकाको निर्णय"
                },
                         new SelectListItem {
                    Text="नगर सभाको निर्णय",
                    Value="नगर सभाको निर्णय"
                },

            };
        }

        public static List<SelectListItem> Swrot()
        {
            return new List<SelectListItem> {
                 new SelectListItem {
                    Text="Select",
                    Value=""
                },

                    new SelectListItem {
                    Text="आन्तरिक श्रोत",
                    Value="आन्तरिक श्रोत"
                },
                new SelectListItem {
                    Text="राजश्वा वाडफाड -प्रदेश सरकार",
                    Value="राजश्वा वाडफाड -प्रदेश सरकार"
                },
                  new SelectListItem {
                    Text="राजश्वा वाडफाड -संघिय सरकार",
                    Value="राजश्वा वाडफाड -संघिय सरकार"
                },
                   new SelectListItem {
                    Text="नेपाल सरकार- समानीकरण आनुदान",
                    Value="नेपाल सरकार- समानीकरण आनुदान"
                },
                    new SelectListItem {
                    Text="संघीय सरकार- समानीकरण आनुदान",
                    Value="संघीय सरकार- समानीकरण आनुदान"
                },
                      new SelectListItem {
                    Text="संघीय सरकार-शसर्त आनुदान",
                    Value="संघीय सरकार-शसर्त आनुदान"
                },
                     new SelectListItem {
                    Text="संघीय सरकार- बिसेष  आनुदान",
                    Value="संघीय सरकार- बिसेष  आनुदान"
                },
                     new SelectListItem {
                    Text="संघीय सरकार- समपुरक आनुदान",
                    Value="संघीय सरकार- समपुरक आनुदान"
                },
                      new SelectListItem {
                    Text="प्रदेश सरकार- समानीकरण आनुदान",
                    Value="प्रदेश सरकार- समानीकरण आनुदान"
                },
                      new SelectListItem {
                    Text="प्रदेश सरकार-शसर्त आनुदान",
                    Value="प्रदेश सरकार-शसर्त आनुदान"
                },
                        new SelectListItem {
                    Text="प्रदेश सरकार- समपुरक आनुदान",
                    Value="प्रदेश सरकार- समपुरक आनुदान"
                },
                       new SelectListItem {
                    Text="अन्तरिक ऋण",
                    Value="अन्तरिक ऋण"
                },


                


                //    new SelectListItem {
                //    Text="Sangiya sarkar bata hastantarit karyakram(samapurak aanudan)",
                //    Value="5"
                //},
            };
        }
    }
}

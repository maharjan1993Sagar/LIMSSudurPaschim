using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class FooterViewModel
    {
        public List<ImportantLinks> ImportantLinks { get; set; }
        public ContactUsModel ContactUs { get; set; }
        public CustomerModel Customer { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class NewsEventViewModel
    {
        public List<NewsEventTenderModel> News { get; set; }
        public NewsEventTenderModel ObjNews { get; set; }
        public List<NewsEventTenderModel> NewsAndEvent { get; set; }

        public string Type { get; set; }
        public string TypeName{ get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Data
{
    public class LimitCount:ILimitCount
    {
        public List<T> TakeFour<T>(List<T> lst) where T : ILimitCount
        {
            return lst.Take(lst.Count > 4 ? 4 : lst.Count).ToList();
        }
    }
}

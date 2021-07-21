using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Data
{
    public interface IGetResource
    {
        string GetByName(string name);
    }
}

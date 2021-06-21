using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Services.Media
{
    public partial interface IMimeMappingService
    {
        string Map(string fName);
    }
}

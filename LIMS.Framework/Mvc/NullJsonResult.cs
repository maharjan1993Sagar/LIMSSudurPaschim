using Microsoft.AspNetCore.Mvc;

namespace LIMS.Framework.Mvc
{
    public class NullJsonResult : JsonResult
    {
        public NullJsonResult() : base(null)
        {
        }
    }
}

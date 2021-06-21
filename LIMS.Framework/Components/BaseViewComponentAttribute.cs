using System;

namespace LIMS.Framework.Components
{
    public class BaseViewComponentAttribute : Attribute
    {
        public bool AdminAccess { get; set; }
    }
}
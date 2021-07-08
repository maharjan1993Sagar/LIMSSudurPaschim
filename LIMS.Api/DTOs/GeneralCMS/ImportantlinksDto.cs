using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class ImportantLinksDto: BaseApiEntityModel
    {
        public Guid ImportantLinkId { get; set; }
        public string LinkName { get; set; }
        public int SerialNo { get; set; }
        public string URL { get; set; }
        public string UserId { get; set; }
    }
}

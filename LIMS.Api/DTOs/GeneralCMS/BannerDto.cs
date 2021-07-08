using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class BannerDto: BaseApiEntityModel
    {
        public string BannerId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public bool IsActive { get; set; }
        public NewsEventFileDto Image { get; set; }
    }
}

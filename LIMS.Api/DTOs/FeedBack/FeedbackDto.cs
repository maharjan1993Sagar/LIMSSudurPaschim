using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.FeedBack
{
    public class FeedbackDto : BaseApiEntityModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
    }
}

using LIMS.Core.ModelBinding;
using LIMS.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class DeathVerificationUploadModel:BaseEntity
    {
        //General Info
        [LIMSResourceDisplayName("Admin.Common.FilePath")]
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public double  FileSize { get; set; }

        [LIMSResourceDisplayName("Admin.Common.File")]
        public IFormFile Image { get; set; }
        public string Id { get; set; }

    }
}

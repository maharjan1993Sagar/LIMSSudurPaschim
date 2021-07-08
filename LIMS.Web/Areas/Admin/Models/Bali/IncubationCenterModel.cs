using LIMS.Core.ModelBinding;
using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class IncubationCenterModel:BaseEntity
    {
        [LIMSResourceDisplayName("LIMS.IncubationCenter.OrganizationName")]
        public string OrganizationNameNepali { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.OrganizationNameEnglish")]

        public string OrganizationNameEnglish { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Province")]

        public string Province { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("LIMS.Common.Address")]

        public string Address { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.RegisteredDate")]
        [UIHint("DateNullable")]
        public DateTime? RegisteredDate { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.RegisteredAddress")]

        public string RegisteredAddress { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.Phone")]

        public string Phone { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.Email")]

        public string Email { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.NatureOfWork")]

        public string NatureOfWork { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.ValueChain")]

        public string ValueChain { get; set; }
        [LIMSResourceDisplayName("LIMS.IncubationCenter.OrganizationStatus")]

        public string OrganizationStatus { get; set; }
       

    }
}

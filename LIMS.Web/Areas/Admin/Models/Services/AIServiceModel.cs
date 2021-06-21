using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class AIServiceModel:BaseEntity
    {
        public AnimalRegistrationModel AnimalRegistration { get; set; }
        
        [LIMSResourceDisplayName("Admin.AI.AiDate")]
        [UIHint("date")]
        public string AIDate { get; set; }

        [LIMSResourceDisplayName("Admin.AI.SemenNo")]

        public string SemenNo { get; set; }
        [LIMSResourceDisplayName("Admin.AI.BullId")]

        public string BullId { get; set; }
        [LIMSResourceDisplayName("Admin.AI.AmountReceived")]

        public string AmountReceived { get; set; }
        [LIMSResourceDisplayName("Admin.AI.ReceiptNo")]

        public string ReceiptNo { get; set; }
        [LIMSResourceDisplayName("Admin.AI.NoofAiDone")]

        public string NoofAiDone{ get; set; }
        [LIMSResourceDisplayName("Admin.Common.Fiscalyear")]

        public string FiscalYear { get; set; }
        [LIMSResourceDisplayName("Admin.common.Technician")]
        public string Technician { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.Species")]
        public string SpeciesId { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.Breed")]
        public string BreedId { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.NoOfWastedSemenDose")]

        public string NoOfWastedSemenDose { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.NoOfAIMade")]

        public string NoOfAIMade { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.TypeOfAi")]

        public string TypeOfAi { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.FarmName")]
        public string FarmName { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.AnimalName")]
        public string AnimalName { get; set; }
        [LIMSResourceDisplayName("Admin.Ai.Eartag")]
        public string Eartag { get; set; }
      
        public string AnimalId { get; set; }
      
        [LIMSResourceDisplayName("Admin.Ai.MobileNo")]
        public string MobileNo { get; set; }

        public string FarmId { get; set; }
    }
}

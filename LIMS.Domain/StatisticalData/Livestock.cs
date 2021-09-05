using LIMS.Domain.AInR;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Domain.Breed
{
    public class Livestock:BaseEntity
    {
        public Livestock()
        {
            Species = new Species();
            Breed = new BreedReg();
            FiscalYear = new FiscalYear();
            Unit = new Unit();
            AnimalType = new AnimalType();
            Farm = new Farm();
        }
        public Species Species { get; set; }

        public BreedReg Breed { get; set; }
        public AnimalType AnimalType { get; set; }
        public string NoOfLivestock { get; set; }
       
        public Unit Unit { get; set; }
       

        public FiscalYear FiscalYear { get; set; }
    
        public string Provience { get; set; }

        
        public string District { get; set; }

        public string LocalLevel { get; set; }

        public string Ward { get; set; }

        
        public string Tole { get; set; }

        public Farm Farm { get; set; }
        public string FarmId { get; set; }
      
        public string Date { get; set; }

        public string Quater { get; set; }
        public string CreatedBy { get; set; }
        public string BreedType { get; set; }
        public string Month { get; set; }
    }
}

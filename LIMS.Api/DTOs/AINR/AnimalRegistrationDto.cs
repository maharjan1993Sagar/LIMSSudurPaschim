using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.AINR
{
    public class AnimalRegistrationDto: BaseApiEntityModel
    {
        public string SpeciesId { get; set; }
        public string BreedId { get; set; }
        public string BreedType { get; set; }
        public string Name { get; set; }
        public string FarmId { get; set; }
        public string EarTagNo { get; set; }

        public string Gender { get; set; }

        public int? Age { get; set; }

        public string SireId { get; set; }

        public string DamId { get; set; }

        public int? Weight { get; set; }

        public string NoOFCalving { get; set; }

        public string PregencyStatus { get; set; }

        public string MilkStatus { get; set; }

        public string PhysicalDefact { get; set; }
    
        public string AnimalColor { get; set; }

        public DateTime? DOB { get; set; }

        public string EntryType { get; set; }

    }
}

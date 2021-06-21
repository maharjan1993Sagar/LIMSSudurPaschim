using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIMS.Domain.AInR
{
    public class FarmShed : SubBaseEntity
    {
        public string FarmId { get; set; }
        public string FarmShedId { get; set; }
        public string Type { get; set; }
        public string Length { get; set; }
        public string Bredth { get; set; }
        public string Height { get; set; }
        public string Volume { get; set; } 
        public DateTime ConstructedDate { get; set; }
        public string Name { get; set; }
    }
}

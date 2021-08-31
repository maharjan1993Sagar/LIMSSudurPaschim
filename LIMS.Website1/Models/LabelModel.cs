using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class LabelModel
    {
        public LabelModel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string EnglishName { get; set; }
        [Required]
        public string NepaliName { get; set; }

    }
}

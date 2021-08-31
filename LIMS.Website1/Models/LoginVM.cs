using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Models
{
    public class LoginVM
    {
        [Required]
        [DisplayName("Login.Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Login.Password")]
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    public class User
    {
        [Key] public string UserId { get; set; }
        [Required] public string Name { get; set; }
        public string Email { get; set; }
        public int EmployeId { get; set; }
        public string Employe { get; set; }
    }
}
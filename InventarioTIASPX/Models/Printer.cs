using InventarioTIASPX.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    [Serializable()]
    public class Printer
    {
        [Key] public string PrinterId { get; set; }
        [StringLength(50)] public string Hostname { get; set; }
        [Required] public string Brand { get; set; }
        [Required] public string Model { get; set; }
        [Required] public string ConnectionType { get; set; }
        [Required] public string Department { get; set; }
        public string UserGUID { get; set; }
        public User User { get; set; }
        public List<FileObject> Files { get; set; }
        public List<Note> Notes { get; set; }
    }
}
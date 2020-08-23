using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    public class Device
    {
        [Key] public string DeviceId { get; set; }
        [Required] public string Type { get; set; }
        [Required] public string Brand { get; set; }
        public string Model { get; set; }
        public string Inventory { get; set; }
        public string ParentComputerId { get; set; }
        [ForeignKey("ParentComputerId")] public Computer ParentComputer { get; set; }
    }
}
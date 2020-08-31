﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace InventarioTIASPX.Models
{
    public class Computer
    {
        [Key] public string ComputerId { get; set; }
        [Required][StringLength(25)][Index(IsUnique = true)] public string Hostname { get; set; }
        [Required] public string Department { get; set; }
        [Required] public string Location { get; set; }
        [Required] public int Architecture { get; set; }
        public string UserId { get; set; }
        public List<Device> Devices { get; set; }
        [ForeignKey("UserId")]public User User { get; set; }
        [ForeignKey("ComputerId")] public Device Processor { get; set; }
    }
}
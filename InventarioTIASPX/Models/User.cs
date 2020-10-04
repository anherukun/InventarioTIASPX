using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    [Serializable()]
    public class User
    {
        [Key] public string UserGUID { get; set; }
        [Required] public long UserId { get; set; }
        [Required] public string Name { get; set; }
        public string Email { get; set; }
        public int EmployeId { get; set; }
        public string Employe { get; set; }
        public bool Migrated { get; set; }
        public List<UserMemberOf> MemberOfs { get; set; }
        public List<Note> Notes { get; set; }
        public List<FileObject> Files { get; set; }
    }
}
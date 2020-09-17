using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    public class UserMemberOf
    {
        [Key] public string UserMemberId { get; set; }
        [Required] public string UserId { get; set; }
        [Required] public string MemberOf { get; set; }
    }
}
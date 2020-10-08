using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using WebGrease.Activities;

namespace InventarioTIASPX.Models
{
    [Serializable()]
    public class FileObject
    {
        [Key] public string FileId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Mime { get; set; }
        [Required] public int Size { get; set; }
        [Required] public byte[] Data { get; set; }
        [Required] public long UploadedTicks { get; set; }
        public string ParentObjectId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    [Serializable()]
    public class Note
    {
        public string NoteId { get; set; }
        public string Body { get; set; }
        public long Ticks { get; set; }
        public string ParentObjectId { get; set; }
    }
}
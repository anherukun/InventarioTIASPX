using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryPrinterNotes : RepositoryNotes
    {
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void AddRange(List<Note> notes)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void BreakRelationship(string noteId)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override List<Note> GetAll()
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void Delete(string noteId)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override Note Get(string noteId)
        {
            throw new NotImplementedException();
        }

        public override void Add(Note note)
        {
            string printerId = note.ParentObjectId;
            note.ParentObjectId = $"PRT_{printerId}";
            using (var db = new InventoryTIASPXContext())
            {
                db.Notes.Add(note);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE notes SET Printer_PrinterId = \"{printerId}\" WHERE noteId LIKE \"{note.NoteId}\"");
            }
        }
        public override List<Note> GetAll(string parentId)
        {
            string parentObjectId = $"PRT_{parentId}";
            using (var db = new InventoryTIASPXContext())
            {
                return db.Notes.Where(x => x.ParentObjectId == parentObjectId).OrderByDescending(x => x.Ticks).ToList();
            }
        }
        public override bool HasNotesRelated(string parentId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                string parentFixed = $"PRT_{parentId}";
                return db.Notes.Any(x => x.ParentObjectId == parentFixed);
            }
        }
    }
}
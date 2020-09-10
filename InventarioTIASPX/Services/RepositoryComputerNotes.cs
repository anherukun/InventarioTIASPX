using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryComputerNotes : RepositoryNotes
    {
        public override void Add(Note note)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.ComputerNotes.Add(note);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE notes SET Computer_ComputerId = \"{note.ParentObjectId}\" WHERE fileId LIKE \"{note.NoteId}\"");
            }
        }

        public override Note Get(string noteId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.ComputerNotes.Where(x => x.NoteId == noteId).FirstOrDefault();
            }
        }

        public override List<Note> GetAll()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.ComputerNotes.ToList();
            }
        }

        public override List<Note> GetAll(string parentId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.ComputerNotes.Where(x => x.ParentObjectId == parentId).ToList();
            }
        }

        public override void Delete(string noteId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(noteId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
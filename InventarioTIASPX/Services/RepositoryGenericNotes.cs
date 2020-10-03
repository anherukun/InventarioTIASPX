using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryGenericNotes : RepositoryNotes
    {
        public override void Add(Note note)
        {
            throw new NotImplementedException();
        }
        public override void AddRange(List<Note> notes)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Notes.AddRange(notes);
                db.SaveChanges();
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

        public override Note Get(string noteId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Notes.Where(x => x.NoteId == noteId).FirstOrDefault();
            }
        }

        public override List<Note> GetAll()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Notes.ToList();
            }
        }

        public override List<Note> GetAll(string parentId)
        {
            throw new NotImplementedException();
        }
    }
}
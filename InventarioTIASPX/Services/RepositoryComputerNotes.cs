using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryComputerNotes : RepositoryNotes
    {
        public override void Add(Note note)
        {
            string computerId = note.ParentObjectId;
            note.ParentObjectId = $"COMP_{computerId}";
            using (var db = new InventoryTIASPXContext())
            {
                db.Notes.Add(note);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE notes SET Computer_ComputerId = \"{computerId}\" WHERE noteId LIKE \"{note.NoteId}\"");
            }
        }
        public override void AddRange(List<Note> notes)
        {
            throw new NotImplementedException();
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
            string parentObjectId = $"COMP_{parentId}";
            using (var db = new InventoryTIASPXContext())
            {
                return db.Notes.Where(x => x.ParentObjectId == parentObjectId).OrderByDescending(x => x.Ticks).ToList();
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

        public override bool HasNotesRelated(string parentId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Notes.Any(x => x.ParentObjectId == $"COMP_{parentId}");
            }
        }

        public override void BreakRelationship(string noteId, string parentId)
        {
            Note note = new RepositoryComputerNotes().Get(noteId);
            note.ParentObjectId = null;

            using (var db = new InventoryTIASPXContext())
            {
                db.Notes.AddOrUpdate(note);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE notes SET Computer_ComputerId = NULL WHERE noteId LIKE \"{note.NoteId}\"");
            }
        }
    }
}
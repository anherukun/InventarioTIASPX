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
    public class RepositoryGenericNotes : RepositoryNotes
    {
        // NO IMPLEMENTADO EN ESTA CLASE
        public override void Add(Note note)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override List<Note> GetAll(string parentId)
        {
            throw new NotImplementedException();
        }
        // NO IMPLEMENTADO EN ESTA CLASE
        public override bool HasNotesRelated(string parentId)
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
        public override void BreakRelationship(string noteId)
        {
            Note note = new RepositoryGenericNotes().Get(noteId);
            note.ParentObjectId = null;

            using (var db = new InventoryTIASPXContext())
            {
                db.Notes.AddOrUpdate(note);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE notes SET Computer_ComputerId = NULL WHERE noteId LIKE \"{note.NoteId}\"");
                db.Database.ExecuteSqlCommand($"UPDATE notes SET User_UserGUID = NULL WHERE noteId LIKE \"{note.NoteId}\"");
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
    }
}
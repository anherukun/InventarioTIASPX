using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryUserNotes : RepositoryNotes
    {
        public override void Add(Note note)
        {
            string userGuid = note.ParentObjectId;
            note.ParentObjectId = $"USR_{userGuid}";

            using (var db = new InventoryTIASPXContext())
            {
                db.Notes.Add(note);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE notes SET User_UserGUID = \"{userGuid}\" WHERE noteId LIKE \"{note.NoteId}\"");
            }
        }
        public override void AddRange(List<Note> notes)
        {
            throw new NotImplementedException();
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
                return db.Notes.Where(x => x.ParentObjectId.Contains("USR_")).ToList();
            }
        }

        public override List<Note> GetAll(string parentId)
        {
            string parentObjectId = $"USR_{parentId}";
            using (var db = new InventoryTIASPXContext())
            {
                return db.Notes.Where(x => x.ParentObjectId == parentObjectId).OrderByDescending(x => x.Ticks).ToList();
            }
        }
    }
}
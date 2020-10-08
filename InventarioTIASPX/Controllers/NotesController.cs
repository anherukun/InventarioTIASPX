using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class NotesController : Controller
    {
        // GET: Notes
        // public ActionResult Index()
        // {
        //     return View();
        // }

        [HttpPost]
        public ActionResult AddComputerNote(string body, string parentId, string path, string controller)
        {
            if (body.Trim().Length > 0)
            {
                new RepositoryComputerNotes().Add(new Note()
                {
                    NoteId = Application.ApplicationManager.GenerateGUID,
                    Body = body.Trim().ToUpper(),
                    Ticks = DateTime.Now.Ticks,
                    ParentObjectId = parentId
                });

                return Redirect(Url.Action(path, controller, new { computerId = parentId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Nota guardada correctamente") }));

            }

            return Redirect(Url.Action(path, controller, new { computerId = parentId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode("No se puede registrar la nota") }));
        }

        [HttpPost]
        public ActionResult AddUserNote(string body, string parentId, string path, string controller)
        {
            User u = RepositoryUser.Get(parentId);

            if (u != null)
            {
                if (body.Trim().Length > 0)
                {
                    new RepositoryUserNotes().Add(new Note()
                    {
                        NoteId = Application.ApplicationManager.GenerateGUID,
                        Body = body.Trim().ToUpper(),
                        Ticks = DateTime.Now.Ticks,
                        ParentObjectId = parentId
                    });

                    return Redirect(Url.Action(path, controller, new { userId = u.UserId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Nota guardada correctamente") }));
                }
            }

            return Redirect(Url.Action("", controller, new { userId = parentId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode("No se puede registrar la nota") }));
        }

        public ActionResult DeleteFromComputersNotes(string noteId, string redirect)
        {
            new RepositoryComputerNotes().Delete(noteId);

            return Redirect(redirect);
        }

        public ActionResult DeleteNote(string noteId, string redirect)
        {
            new RepositoryGenericNotes().Delete(noteId);

            return Redirect(redirect);
        }
    }
}
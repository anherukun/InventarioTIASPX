using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class FilesController : Controller
    {
        // GET: Files
        // public ActionResult Index()
        // {
        //     return View();
        // }

        [HttpPost]
        public ActionResult UploadComputerFile(HttpPostedFileBase file, string parentId, string path, string controller)
        {
            try
            {
                if (file != null)
                {
                    FileObject fileObject = new FileObject()
                    {
                        FileId = Application.ApplicationManager.GenerateGUID,
                        ParentObjectId = parentId,
                        UploadedTicks = DateTime.Now.Ticks,
                        Mime = file.ContentType,
                        Name = file.FileName,
                        Size = file.ContentLength
                    };

                    fileObject.Data = Application.ApplicationManager.Compress(new BinaryReader(file.InputStream).ReadBytes(file.ContentLength));

                    new RepositoryComputerFiles().Add(fileObject);
                    return Redirect(Url.Action(path, controller, new { computerId = parentId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("El archivo se cargo correctamente") }));
                } else
                {
                    throw new Exception("Primero debes seleccionar un archivo");
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action(path, controller, new { computerId = parentId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
            }
        }

        [HttpPost]
        public ActionResult UploadUserFile(HttpPostedFileBase file, string parentId, string path, string controller)
        {
            try
            {
                User u = RepositoryUser.Get(parentId);
                if (u != null)
                    if (file != null)
                    {
                        FileObject fileObject = new FileObject()
                        {
                            FileId = Application.ApplicationManager.GenerateGUID,
                            ParentObjectId = parentId,
                            UploadedTicks = DateTime.Now.Ticks,
                            Mime = file.ContentType,
                            Name = file.FileName,
                            Size = file.ContentLength
                        };

                        fileObject.Data = Application.ApplicationManager.Compress(new BinaryReader(file.InputStream).ReadBytes(file.ContentLength));

                        new RepositoryUserFiles().Add(fileObject);
                        return Redirect(Url.Action(path, controller, new { userId = u.UserId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("El archivo se cargo correctamente") }));
                    }
                    else
                    {
                        throw new Exception("Primero debes seleccionar un archivo");
                    }
                else
                {
                    throw new Exception("El usuario especificado no existe");
                }

            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("", controller, new { userId = parentId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
            }
        }

        [HttpPost]
        public ActionResult UploadPrinterFile(HttpPostedFileBase file, string parentId, string path, string controller)
        {
            try
            {
                Printer p = RepositoryPrinter.Get(parentId);
                if (p != null)
                    if (file != null)
                    {
                        FileObject fileObject = new FileObject()
                        {
                            FileId = Application.ApplicationManager.GenerateGUID,
                            ParentObjectId = parentId,
                            UploadedTicks = DateTime.Now.Ticks,
                            Mime = file.ContentType,
                            Name = file.FileName,
                            Size = file.ContentLength
                        };

                        fileObject.Data = Application.ApplicationManager.Compress(new BinaryReader(file.InputStream).ReadBytes(file.ContentLength));

                        new RepositoryPrinterFiles().Add(fileObject);
                        return Redirect(Url.Action(path, controller, new { printerId = p.PrinterId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("El archivo se cargo correctamente") }));
                    }
                    else
                    {
                        throw new Exception("Primero debes seleccionar un archivo");
                    }
                else
                {
                    throw new Exception("El usuario especificado no existe");
                }

            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("", controller, new { printerId = parentId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
            }
        }

        [HttpGet]
        public FileResult DownloadFile(string fileId)
        {
            FileObject file = new RepositoryGenericFiles().Get(fileId, true);

            if (file != null)
                return File(Application.ApplicationManager.Decompress(file.Data), file.Mime, file.Name);
            else
                return null;
        }
        [HttpGet]
        public ActionResult DeleteFromComputer(string fileId, string computerId)
        {
            if (fileId != null)
            {
                ViewData["file"] = new RepositoryGenericFiles().Get(fileId, false);
                ViewData["computer"] = RepositoryComputer.Get(computerId);
                return View();
            }
            else
                return Redirect(Url.Action("", "Computers"));
        }
        [HttpGet]
        public ActionResult DeleteFromUser(string fileId, long userId)
        {
            if (fileId != null)
            {
                ViewData["file"] = new RepositoryGenericFiles().Get(fileId, false);
                ViewData["user"] = RepositoryUser.Get(userId);
                return View();
            }
            else
                return Redirect(Url.Action("", "Users"));
        }
        [HttpGet]
        public ActionResult DeleteFromPrinter(string fileId, string printerId)
        {
            if (fileId != null)
            {
                ViewData["file"] = new RepositoryGenericFiles().Get(fileId, false);
                ViewData["printer"] = RepositoryPrinter.Get(printerId);
                return View();
            }
            else
                return Redirect(Url.Action("", "Printers"));
        }

        [HttpPost]
        public ActionResult Delete(string fileId, string redirect)
        {
            new RepositoryGenericFiles().Delete(fileId);

            return Redirect(redirect);
        }
    }
}
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
        public ActionResult Index()
        {
            return View();
        }

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

                    fileObject.Data = new BinaryReader(file.InputStream).ReadBytes(file.ContentLength);

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

        public FileResult DownloadFromComputersFiles(string fileId)
        {
            FileObject file = new RepositoryComputerFiles().Get(fileId);

            if (file != null)
                return File(file.Data, file.Mime, file.Name);
            else
                return null;
        }

        [HttpGet]
        public ActionResult DeleteFromComputersFiles(string fileId, string redirect)
        {
            new RepositoryComputerFiles().Delete(fileId);

            return Redirect(redirect);
        }
    }
}
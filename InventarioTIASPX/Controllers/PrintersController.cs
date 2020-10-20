using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class PrintersController : Controller
    {
        // GET: Printers
        public ActionResult Index()
        {
            ViewData["printers"] = RepositoryPrinter.GetAll();
            ViewData["departments"] = RepositoryPrinter.GetAllDepartments();
            ViewData["locations"] = RepositoryPrinter.GetAllLocations();
            ViewData["brands"] = RepositoryPrinter.GetAllBrands();
            ViewData["models"] = RepositoryPrinter.GetAllModels();

            return View();
        }

        [HttpGet]
        public ActionResult Printer(string printerId, string msgType, string msgString)
        {
            if (RepositoryPrinter.Exist(printerId))
            {
                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                ViewData["printer"] = RepositoryPrinter.Get(printerId); 
                ViewData["files"] = new RepositoryPrinterFiles().GetAll(printerId);
                ViewData["notes"] = new RepositoryPrinterNotes().GetAll(printerId);

                return View();
            }

            return Redirect(Url.Action("", "Printers"));
        }

        [HttpGet]
        public ActionResult NewPrinter(string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            ViewData["departments"] = RepositoryPrinter.GetAllDepartments();
            ViewData["locations"] = RepositoryPrinter.GetAllLocations();
            ViewData["brands"] = RepositoryPrinter.GetAllBrands();
            ViewData["models"] = RepositoryPrinter.GetAllModels();
            ViewData["users"] = RepositoryUser.GetAllUsers();

            return View();
        }

        [HttpGet]
        public ActionResult EditPrinter(string printerId, string msgType, string msgString)
        {
            if (RepositoryPrinter.Exist(printerId))
            {
                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                ViewData["printer"] = RepositoryPrinter.Get(printerId);
                ViewData["departments"] = RepositoryPrinter.GetAllDepartments();
                ViewData["locations"] = RepositoryPrinter.GetAllLocations();
                ViewData["brands"] = RepositoryPrinter.GetAllBrands();
                ViewData["models"] = RepositoryPrinter.GetAllModels();
                ViewData["users"] = RepositoryUser.GetAllUsers();

                return View();
            }

            return Redirect(Url.Action("", "Printers"));
        }

        [HttpGet]
        public ActionResult DeletePrinter(string printerId, string msgType, string msgString)
        {
            if (RepositoryPrinter.Exist(printerId))
            {
                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                ViewData["printer"] = RepositoryPrinter.Get(printerId);

                return View();
            }

            return Redirect(Url.Action("", "Printers"));
        }

        [HttpPost]
        public ActionResult Add(Printer printer)
        {
            if (printer != null)
                if (!RepositoryPrinter.Exist(printer.PrinterId))
                {
                    try
                    {
                        RepositoryPrinter.Add(printer);

                        return Redirect(Url.Action("NewPrinter", "Printers", new { msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Se agrego la impresora correctamente") }));
                    }
                    catch (Exception ex)
                    {
                        return Redirect(Url.Action("NewPrinter", "Printers", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
                    }
                }
                else
                    return Redirect(Url.Action("NewPrinter", "Printers", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode("Ya existe esta impresora") }));

            return Redirect(Url.Action("NewPrinter", "Printers", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode("Opss... Algo salio mal, vuelve a intentarlo") }));
        }

        [HttpPost]
        public ActionResult Edit(Printer printer)
        {
            try
            {
                // SE MANIENE UNA COPIA EN MEMORIA DE LA ENTIDAD ORIGINAL
                Printer originalEntity = RepositoryPrinter.Get(printer.PrinterId);

                // SE CAMBIAN LOS VALORES ORIGINALES POR LOS NUEVOS
                originalEntity.ConnectionType = printer.ConnectionType;
                originalEntity.Hostname = printer.Hostname;
                originalEntity.Model = printer.Model;
                originalEntity.Brand = printer.Brand;
                originalEntity.Department = printer.Department;
                originalEntity.Location = printer.Location;
                originalEntity.UserGUID = printer.UserGUID;

                // REGISTRA LOS CAMBIOS EN EL REPOSITORIO
                RepositoryPrinter.Update(originalEntity);

                return Redirect(Url.Action("Printer", "Printers", new { printerId = printer.PrinterId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Se guardaron los cambios correctamente") }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("Printer", "Printers", new { printerId = printer.PrinterId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
            }
        }

        [HttpPost]
        public ActionResult Delete(string printerId)
        {
            RepositoryPrinter.Delete(printerId);

            return Redirect(Url.Action("", "Printers"));
        }
    }
}
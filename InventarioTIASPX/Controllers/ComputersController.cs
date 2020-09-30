using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class ComputersController : Controller
    {
        // GET: Computers
        public ActionResult Index()
        {
            ViewData["computers"] = RepositoryComputer.GetAllComputers();
            ViewData["departments"] = RepositoryComputer.GetAllDepartments();

            return View();
        }

        [HttpGet]
        public ActionResult Computer(string computerId, string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }
            if (computerId != null)
                if ((ViewData["computer"] = RepositoryComputer.Get(computerId)) != null)
                {
                    ViewData["files"] = new RepositoryComputerFiles().GetAll(computerId);
                    ViewData["notes"] = new RepositoryComputerNotes().GetAll(computerId);
                    return View();
                }

            return Redirect(Url.Action("", "Computers"));
        }

        public ActionResult NewComputer(string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            ViewData["processors"] = RepositoryDevice.GetAllProcessors(false);
            ViewData["departments"] = RepositoryComputer.GetAllDepartments();
            ViewData["users"] = RepositoryUser.GetAllUsers();
            ViewData["accesories"] = RepositoryDevice.GetAllAccesories(false);

            return View();
        }

        public ActionResult EditComputer(string computerId, string msgType, string msgString)
        {
            if (computerId != null)
            {
                // DETECCION DEL MENSAJE
                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                // DETECCION DE ACCESORIOS RELACIONADOS
                if (RepositoryComputer.Exist(computerId))
                {
                    ViewData["computer"] = RepositoryComputer.Get(computerId);

                    List<Device> computerAccesories = new List<Device>();

                    foreach (var item in (ViewData["computer"] as Computer).Devices)
                        computerAccesories.Add(RepositoryDevice.Get(item.DeviceId, false));

                    ViewData["computerAccesories"] = computerAccesories;

                    ViewData["processor"] = RepositoryDevice.Get(computerId, true);
                    ViewData["departments"] = RepositoryComputer.GetAllDepartments();
                    ViewData["users"] = RepositoryUser.GetAllUsers();
                    ViewData["accesories"] = RepositoryDevice.GetAllAccesories();

                    return View();
                }
                else
                    return Redirect(Url.Action("", "Computers"));

            }
            else
                return Redirect(Url.Action("", "Computers"));
        }

        [HttpPost]
        public ActionResult Add(Computer computer, string jsonDevices)
        {
            List<Device> devices = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Device>>(jsonDevices);

            try
            {
                RepositoryComputer.Add(computer);

                if (devices != null)
                    foreach (var item in devices)
                    {
                        RepositoryDevice.AssignComputer(item.DeviceId, computer.ComputerId);
                    }

                return Redirect(Url.Action("NewComputer", "Computers", new { msgType = "success", msgString = Application.ApplicationManager.Base64Encode("La computadora fue registrada correctamente") }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("NewComputer", "Computers", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) } ));
            }
        }

        [HttpPost]
        public ActionResult Edit(Computer computer, string jsonDevices)
        {
            List<Device> fromEditDevices = JsonConvert.DeserializeObject<List<Device>>(jsonDevices);

            return (Redirect(Url.Action("Comupter", "Computers", new { computerId = computer.ComputerId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Los cambios se guardaron correctamente") })));
        }

        public ActionResult Delete(string computerId)
        {
            RepositoryComputer.Delete(computerId);
            return Redirect(Url.Action("", "Computers"));
        }
    }
}
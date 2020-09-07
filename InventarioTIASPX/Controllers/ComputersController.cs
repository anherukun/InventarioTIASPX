using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using Microsoft.Ajax.Utilities;
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
                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                ViewData["computer"] = RepositoryComputer.Get(computerId);
                if ((ViewData["computer"] as Computer).Devices != null)
                {
                    List<Device> computerAccesories = new List<Device>();

                    foreach (var item in (ViewData["computer"] as Computer).Devices)
                        computerAccesories.Add(RepositoryDevice.GetWithoutInclude(item.DeviceId));

                    ViewData["computerAccesories"] = computerAccesories;
                }
                
                ViewData["processor"] = RepositoryDevice.Get(computerId);
                ViewData["departments"] = RepositoryComputer.GetAllDepartments();
                ViewData["users"] = RepositoryUser.GetAllUsers();
                ViewData["availableAccesories"] = RepositoryDevice.GetAllAccesories(true);
                ViewData["accesories"] = RepositoryDevice.GetAllAccesories(false);

                return View();
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
                foreach (var item in devices)
                {
                    RepositoryDevice.AssignComputer(item.DeviceId, computer.ComputerId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return Redirect(Url.Action("NewComputer", "Computers"));
        }

        public ActionResult Delete(string computerId)
        {
            RepositoryComputer.Delete(computerId);
            return Redirect(Url.Action("", "Computers"));
        }
    }
}
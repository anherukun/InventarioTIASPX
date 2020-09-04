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

        public ActionResult Computer(string computerId)
        {
            if (computerId != null)
                if ((ViewData["computer"] = RepositoryComputer.Get(computerId)) != null)
                    return View();

            return Redirect(Url.Action("", "Computers"));
        }

        public ActionResult NewComputer()
        {
            ViewData["processors"] = RepositoryDevice.GetAllProcessors(false);
            ViewData["departments"] = RepositoryComputer.GetAllDepartments();
            ViewData["users"] = RepositoryUser.GetAllUsers();
            ViewData["accesories"] = RepositoryDevice.GetAllAccesories(false);
            return View();
        }

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
    }
}
using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class DevicesController : Controller
    {
        // GET: Devices
        public ActionResult Index()
        {
            ViewData["devices"] = RepositoryDevice.GetAllDevices();
            ViewData["devicetypes"] = RepositoryDevice.GetAllDeviceTypes();
            ViewData["deviceBrands"] = RepositoryDevice.GetAllDeviceBrands();
            ViewData["deviceModels"] = RepositoryDevice.GetAllDeviceModels();
            return View();
        }

        public ActionResult NewDevice()
        {
            ViewData["devicetypes"] = RepositoryDevice.GetAllDeviceTypes();
            ViewData["deviceBrands"] = RepositoryDevice.GetAllDeviceBrands();
            ViewData["deviceModels"] = RepositoryDevice.GetAllDeviceModels();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Device device)
        {
            RepositoryDevice.Add(device);
            return Redirect(Url.Action("NewDevice", "Devices"));
        }

        public ActionResult Delete(string deviceId)
        {
            RepositoryDevice.Delete(deviceId);
            return Redirect(Url.Action("Index", "Devices"));
        }
    }
}
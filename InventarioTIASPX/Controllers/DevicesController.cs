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

        [HttpGet]
        public ActionResult NewDevice(string msgType, string msgString)
        {
            ViewData["devicetypes"] = RepositoryDevice.GetAllDeviceTypes();
            ViewData["deviceBrands"] = RepositoryDevice.GetAllDeviceBrands();
            ViewData["deviceModels"] = RepositoryDevice.GetAllDeviceModels();
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(Device device)
        {
            try
            {
                RepositoryDevice.Add(device);
                return Redirect(Url.Action("NewDevice", "Devices", new { msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Se agrego el dispositivo correctamente")}));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("NewDevice", "Devices", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
                throw;
            }
            
        }

        public ActionResult Delete(string deviceId)
        {
            RepositoryDevice.Delete(deviceId);
            return Redirect(Url.Action("Index", "Devices"));
        }
    }
}
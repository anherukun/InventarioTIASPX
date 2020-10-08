using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

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
        public ActionResult Device(string deviceId, string msgType, string msgString)
        {
            if (deviceId != null)
            {
                Device device = RepositoryDevice.Get(deviceId, true);
                ViewData["device"] = device;
                
                if (device.ParentComputerId != null && RepositoryComputer.Exist(device.ParentComputerId))
                {
                    ViewData["computer"] = RepositoryComputer.Get(device.ParentComputerId);
                }

                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                return View();
            }
            else
                return Redirect(Url.Action("", "Devices"));
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

        [HttpGet]
        public ActionResult EditDevice(string deviceId, string msgType, string msgString)
        {
            if (deviceId != null)
            {
                ViewData["device"] = RepositoryDevice.Get(deviceId, true);
                ViewData["devicetypes"] = RepositoryDevice.GetAllDeviceTypes();
                ViewData["deviceBrands"] = RepositoryDevice.GetAllDeviceBrands();
                ViewData["deviceModels"] = RepositoryDevice.GetAllDeviceModels();
                if (msgType != null && msgString != null)
                {
                    msgString = Application.ApplicationManager.Base64Decode(msgString);
                    ViewData["message"] = new { msgType, msgString };
                }

                return View();
            } else
                return Redirect(Url.Action("", "Devices"));
        }

        [HttpGet]
        public ActionResult DeleteDevice(string deviceId)
        {
            if (deviceId != null)
            {
                if ((ViewData["device"] = RepositoryDevice.Get(deviceId, false)) != null)
                {
                    return View();
                }
                return Redirect(Url.Action("", "Devices"));
            }
            else
                return Redirect(Url.Action("", "Devices"));
        }

        [HttpPost]
        public ActionResult Delete(string deviceId)
        {
            RepositoryDevice.Delete(deviceId);

            return Redirect(Url.Action("", "Devices"));
        }

        [HttpGet]
        public ActionResult UnassignComputer(string deviceId, string redirect)
        {
            RepositoryDevice.UnassignComputer(deviceId);

            return Redirect(redirect);
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

        [HttpPost]
        public ActionResult Edit(Device device)
        {
            try
            {
                RepositoryDevice.Update(device);
                return Redirect(Url.Action("Device", "Devices", new { deviceId = device.DeviceId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Se modifico el dispositivo correctamente") }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("EditDevice", "Devices", new { deviceId = device.DeviceId , msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
                throw;
            }
        }
    }
}
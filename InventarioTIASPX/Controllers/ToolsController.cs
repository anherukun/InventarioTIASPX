using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class ToolsController : Controller
    {
        // GET: Tools
        public ActionResult Index()
        {
            return Redirect(Url.Action("ImportData", "Tools"));
            //return View();
        }

        [HttpGet]
        public ActionResult ImportData(string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            return View();
        }

        [HttpPost]
        public ActionResult ImportFromCSV(HttpPostedFileBase fileDevices, char charSplit)
        {
            try
            {
                if (fileDevices != null)
                {
                    using (var reader = new BinaryReader(fileDevices.InputStream))
                    {
                        byte[] data = reader.ReadBytes(fileDevices.ContentLength);

                        // 0: SERIE | 1: INVENTARIO | 2: TIPO | 3: MARCA | 4: MODELO
                        List<Device> devices = new List<Device>();
                        // CONVIERTE LOS BYTES EN STRING
                        string stringData = System.Text.Encoding.UTF8.GetString(data).ToString();
                        // SEPARA LAS LINEAS EN UNA LISTA DE STRINGS
                        List<string> rows = stringData.Split('\n').ToList();
                        // QUITA LOS ENCABEZADOS DE LA TABLA
                        rows.RemoveAt(0);
                        // RECORRE CADA LINEA Y CREA UN DEVICE NUEVO AGREGANDOLO A LA LISTA DE DEVICES
                        foreach (var item in rows)
                        {
                            if (item.Split(charSplit)[0].Trim().Length > 0 && item.Split(charSplit)[2].Trim().Length > 0 && item.Split(charSplit)[3].Trim().Length > 0)
                            {
                                Device d = new Device()
                                {
                                    DeviceId = item.Split(charSplit)[0].Trim().ToUpper(),
                                    Type = item.Split(charSplit)[2].Trim().ToUpper(),
                                    Brand = item.Split(charSplit)[3].Trim().ToUpper()
                                };

                                if (item.Split(charSplit)[1].Trim().Length > 0)
                                    d.Inventory = item.Split(charSplit)[1].Trim().ToUpper();
                                if (item.Split(charSplit)[4].Trim().Length > 0)
                                    d.Model = item.Split(charSplit)[4].Trim().ToUpper();

                                devices.Add(d);
                            }
                        }

                        // SI HAY POR LO MENOS UN DISPOSITIVO NUEVO, SE ENVIA AL REPOSITORIO Y SE GUARDA EN LA BASE DE DATOS
                        if (devices.Count > 0)
                            RepositoryDevice.AddRange(devices);

                        reader.Dispose();
                    }
                    // BinaryReader reader = new BinaryReader(fileDevices.InputStream);
                    
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("ImportData", "Tools", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
            }
            
            return Redirect(Url.Action("ImportData", "Tools", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode("Datos importados correctamente") }));
        }
    }
}
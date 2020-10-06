using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Security.Permissions;
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

        [HttpGet]
        public ActionResult Backup(string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            return View();
        }

        [HttpGet]
        public FileResult CreateBackup()
        {
            Backup backup = new Backup(RepositoryDevice.GetAllDevices(), RepositoryUser.GetAllUsers(), RepositoryComputer.GetAllComputers(), new RepositoryGenericNotes().GetAll(), new RepositoryGenericFiles().GetAllWithData());

            if (backup != null)
            {
                byte[] backupData = Application.ApplicationManager.Compress(backup.ToBytes());

                return File(backupData, "application/octet-stream", $"{DateTime.Now.Ticks}_{DateTime.Now.ToShortDateString()}_Backup.bak");
            }
            else
            {
                throw new NullReferenceException("No se puede crear el archivo a partir de un objeto nulo");
            }
        }

        [HttpPost]
        public ActionResult Recover(HttpPostedFileBase fileBackup)
        {
            if (fileBackup != null)
            {
                DateTime dt1 = DateTime.Now, dt2;
                // Descomprime el archivo y lo almacena en un arreglo de bytes
                byte[] backupData = Application.ApplicationManager.Decompress(new BinaryReader(fileBackup.InputStream).ReadBytes(fileBackup.ContentLength));
                // Lee los bytes del archivo .bak y los deserializa a un objeto Backup
                Backup backup = new Backup().FromBytes(backupData);
                // Llama al servicio de Backup y empieza la recuperacion de datos
                BackupService.RecoverData(backup);

                dt2 = DateTime.Now;
                TimeSpan span = dt2.Subtract(dt1);
                return Redirect(Url.Action("Backup", "Tools", new { msgType = "success", msgString = Application.ApplicationManager.Base64Encode($"Los datos fueron restaurados correctamente \nT: {span.ToString()}") }));
            }
            else
                return Redirect(Url.Action("Backup", "Tools", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode("No se puede recuperar de un objeto nulo") }));
        }

        [HttpPost]
        public ActionResult ImportFromCSV(HttpPostedFileBase fileDevices, HttpPostedFileBase fileUsers, HttpPostedFileBase fileComputers, HttpPostedFileBase fileAccesories, char charSplit)
        {
            DateTime a = DateTime.Now;
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
                            // VERIFICA QUE EXISTAN DATOS EN LOS CAMPOS IMPORTANTES
                            if (item.Split(charSplit)[0].Trim().Length > 0 && item.Split(charSplit)[2].Trim().Length > 0 && item.Split(charSplit)[3].Trim().Length > 0)
                                // VERIFICA QUE NO EXISTA YA UN REGISTRO CON EL MISMO NUMERO DE SERIE
                                if (!RepositoryDevice.Exist(item.Split(charSplit)[0].Trim().ToUpper()))
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
                }

                if (fileUsers != null)
                {
                    using (var reader = new BinaryReader(fileUsers.InputStream))
                    {
                        byte[] data = reader.ReadBytes(fileUsers.ContentLength);

                        // 0: USUARIO | 1: NOMBRE DE CUENTA | 2: EMAIL | 3: FICHA | 4: NOMBRE DE EMPLEADO | 5: MIGRADO (1 : 0)
                        List<User> users = new List<User>();
                        // CONVIERTE LOS BYTES EN STRING
                        string stringData = System.Text.Encoding.UTF8.GetString(data).ToString();
                        // SEPARA LAS LINEAS EN UNA LISTA DE STRINGS
                        List<string> rows = stringData.Split('\n').ToList();
                        // QUITA LOS ENCABEZADOS DE LA TABLA
                        rows.RemoveAt(0);
                        // RECORRE CADA LINEA Y CREA UN USER NUEVO AGREGANDOLO A LA LISTA DE DEVICES
                        foreach (var item in rows)
                        {
                            // VERIFICA QUE HAYA DATOS EN LOS CAMPOS IMPORTANTES
                            if (item.Split(charSplit)[0].Trim().Length > 0 && item.Split(charSplit)[1].Trim().Length > 0 && item.Split(charSplit)[3].Trim().Length > 0)
                                // VERIFICA QUE NO EXISTA UN REGISTRO CON EL MISMO USUARIO
                                if (!RepositoryUser.Exist(long.Parse(item.Split(charSplit)[0].Trim())))
                                {
                                    User u = new Models.User()
                                    {
                                        UserGUID = Application.ApplicationManager.GenerateGUID,
                                        UserId = long.Parse(item.Split(charSplit)[0].Trim()),
                                        Name = item.Split(charSplit)[1].Trim().ToUpper(),
                                        EmployeId = int.Parse(item.Split(charSplit)[3].Trim()),
                                        Employe = item.Split(charSplit)[4].Trim().ToUpper()
                                    };

                                    u.Email = item.Split(charSplit)[2].Trim().Length > 0 ? item.Split(charSplit)[2].Trim().ToUpper() : null;
                                    u.Migrated = item.Split(charSplit)[5].Trim() == "1" ? true : false;

                                    users.Add(u);
                                }
                        }

                        // SI HAY POR LO MENOS UN DISPOSITIVO NUEVO, SE ENVIA AL REPOSITORIO Y SE GUARDA EN LA BASE DE DATOS
                        if (users.Count > 0)
                            RepositoryUser.AddRange(users);

                        reader.Dispose();
                    }
                }

                if (fileComputers != null)
                {
                    using (var reader = new BinaryReader(fileComputers.InputStream))
                    {
                        byte[] data = reader.ReadBytes(fileComputers.ContentLength);

                        // 0: SERIE PROCESADOR | 1: HOSTNAME | 2: DEPARTAMENTO | 3: UBICACION FISICA | 4: ARQUITECTURA | 5: USUARIO
                        List<Computer> computers = new List<Computer>();
                        // CONVIERTE LOS BYTES EN STRING
                        string stringData = System.Text.Encoding.UTF8.GetString(data).ToString();
                        // SEPARA LAS LINEAS EN UNA LISTA DE STRINGS
                        List<string> rows = stringData.Split('\n').ToList();
                        // QUITA LOS ENCABEZADOS DE LA TABLA
                        rows.RemoveAt(0);
                        // RECORRE CADA LINEA Y CREA UN COMPUTER NUEVO AGREGANDOLO A LA LISTA DE COMPUTERS
                        foreach (var item in rows)
                        {
                            // VERIFICA QUE HAYA DATOSN EN LOS CAMPOS IMPORTANTES
                            if (item.Split(charSplit)[0].Trim().Length > 0 && item.Split(charSplit)[2].Trim().Length > 0 && item.Split(charSplit)[3].Trim().Length > 0)
                                // VERIFICA QUE EXISTA EL PROCESADOR EN LOS DISPOSITIVOS
                                if (RepositoryDevice.Exist(item.Split(charSplit)[0].Trim()))
                                    // VERIFICA QUE NO EXISTA UN REGISTRO DEL MISMO PROCESADOR EN LAS COMPUTADORAS
                                    if (!RepositoryComputer.Exist(item.Split(charSplit)[0].Trim()))
                                    {
                                        Computer c = new Computer()
                                        {
                                            ComputerId = item.Split(charSplit)[0].Trim().ToUpper(),
                                            Hostname = item.Split(charSplit)[1].Trim().ToUpper(),
                                            Department = item.Split(charSplit)[2].Trim().ToUpper(),
                                            Location = item.Split(charSplit)[3].Trim().ToUpper(),
                                            Architecture = int.Parse(item.Split(charSplit)[4].Trim())
                                        };
                                        if (item.Split(charSplit)[5].Trim().Length > 0)
                                            if (RepositoryUser.Exist(long.Parse(item.Split(charSplit)[5].Trim())))
                                                c.UserGUID = RepositoryUser.Get(long.Parse(item.Split(charSplit)[5].Trim())).UserGUID;

                                        computers.Add(c);
                                    }
                        }

                        // SI HAY POR LO MENOS UN DISPOSITIVO NUEVO, SE ENVIA AL REPOSITORIO Y SE GUARDA EN LA BASE DE DATOS
                        if (computers.Count > 0)
                            RepositoryComputer.AddRange(computers);

                        reader.Dispose();
                    }
                }

                if (fileAccesories != null)
                {
                    using (var reader = new BinaryReader(fileAccesories.InputStream))
                    {
                        byte[] data = reader.ReadBytes(fileAccesories.ContentLength);

                        // 0: SERIE PROCESADOR | 1: SERIE ACCESORIO
                        List<Computer> computers = new List<Computer>();
                        // CONVIERTE LOS BYTES EN STRING
                        string stringData = System.Text.Encoding.UTF8.GetString(data).ToString();
                        // SEPARA LAS LINEAS EN UNA LISTA DE STRINGS
                        List<string> rows = stringData.Split('\n').ToList();
                        // QUITA LOS ENCABEZADOS DE LA TABLA
                        rows.RemoveAt(0);
                        // RECORRE CADA LINEA ASIGNANDO EL DISPOSITIVO AL COMPUTER
                        foreach (var item in rows)
                        {
                            // VERIFICA QUE HAYA DATOS EN LOS CAMPOS IMPORTANTES
                            if (item.Split(charSplit)[0].Trim().Length > 0 && item.Split(charSplit)[1].Trim().Length > 0)
                            {
                                // VERIFICA QUE EXISTA EL REGISTRO DE COMPUTADOR
                                if (RepositoryComputer.Exist(item.Split(charSplit)[0].Trim()))
                                    // VERIFICA QUE EXISTA EL REGISTRO DEL DISPOSITIVO
                                    if (RepositoryDevice.Exist(item.Split(charSplit)[1].Trim()))
                                        // VERIFICA QUE EL DISPOSITIVO NO ESTE EN USO
                                        if (!RepositoryDevice.Get(item.Split(charSplit)[1].Trim(), true).InUse)
                                            // ASIGNA EL DISPOSITIVO A LA COMPUTADORA
                                            RepositoryDevice.AssignComputer(item.Split(charSplit)[1].Trim(), item.Split(charSplit)[0].Trim());
                            }
                        }

                        reader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("ImportData", "Tools", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode(ex.Message) }));
            }

            DateTime b = DateTime.Now;
            TimeSpan span = b.Subtract(a);
            return Redirect(Url.Action("ImportData", "Tools", new { msgType = "success", msgString = Application.ApplicationManager.Base64Encode($"Datos importados correctamente \nT: {span.ToString()}") }));
        }

    }
}
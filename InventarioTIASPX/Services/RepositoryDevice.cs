using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryDevice
    {
        public static void Add(Device device)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Devices.AddOrUpdate(device);
                db.SaveChanges();
            }
        }

        public static void AddRange(List<Device> devices)
        {
            try
            {
                using (var db = new InventoryTIASPXContext())
                {
                    db.Devices.AddRange(devices);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException.InnerException;
            }
        }

        public static void Update(Device device)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Devices.AddOrUpdate(device);
                db.SaveChanges();
            }
        }

        public static Device Get(string deviceId, bool useInclude)
        {
            using (var db = new InventoryTIASPXContext())
            {
               // if (useInclude)
               //     return db.Devices.Where(x => x.DeviceId == deviceId).Include(x => x.ParentComputer).FirstOrDefault();
               // else
                    return db.Devices.Where(x => x.DeviceId == deviceId).FirstOrDefault();
            }
        }

        public static List<Device> GetAllDevices()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.OrderBy(x => x.Model).OrderBy(x => x.Brand).OrderBy(x => x.Type).ToList();
            }
        }

        public static List<Device> GetAllProcessors()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.OrderBy(x => x.Model).OrderBy(x => x.Brand).OrderBy(x => x.Type)
                    .Where(x => x.Type == "PROCESADOR" || x.Type == "LAPTOP")
                    .ToList();
            }
        }
        public static List<Device> GetAllProcessors(bool inUse)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.OrderBy(x => x.Model).OrderBy(x => x.Brand).OrderBy(x => x.Type)
                    .Where(x => x.Type == "PROCESADOR" || x.Type == "LAPTOP")
                    .Where(x => x.InUse == inUse)
                    .ToList();
            }
        }
        public static List<Device> GetAllAccesories()
        {
            List<Device> devices = new List<Device>();
            List<string> types = RepositoryDevice.GetAllDeviceTypes();
            types.Remove("PROCESADOR");
            types.Remove("LAPTOP");

            foreach (var item in types)
            {
                devices.AddRange(GetAllByType(item));
            }

            return devices;
        }
        public static List<Device> GetAllAccesories(bool inUse)
        {
            List<Device> devices = new List<Device>();
            List<string> types = RepositoryDevice.GetAllDeviceTypes();
            types.Remove("PROCESADOR");
            types.Remove("LAPTOP");

            foreach (var item in types)
            {
                devices.AddRange(GetAllByType(inUse, item));
            }

            return devices;
        }
        public static List<Device> GetAllByType(string type)
        {
            List<Device> result = new List<Device>();
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.Where(x => x.Type == type)
                    .OrderBy(x => x.Brand)
                    .ToList();
            }
        }
        public static List<Device> GetAllByType(bool inUse, string type)
        {
            List<Device> result = new List<Device>();
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.Where(x => x.InUse == inUse && x.Type == type)
                    .OrderBy(x => x.Brand)
                    .ToList();
            }
        }

        public static List<string> GetAllDeviceTypes()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.Select(x => x.Type).Distinct().OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllDeviceBrands()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.Select(x => x.Brand).Distinct().OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllDeviceModels()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.Select(x => x.Model).Distinct().OrderBy(x => x).ToList();
            }
        }

        public static void AssignComputer(string deviceId, string computerId)
        {
            Device device = RepositoryDevice.Get(deviceId, true);
            device.ParentComputerId = computerId;
            device.InUse = true;

            using (var db = new InventoryTIASPXContext())
            {
                db.Devices.AddOrUpdate(device);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE devices SET Computer_ComputerId = \"{computerId}\" WHERE deviceId LIKE \"{deviceId}\"");
            }
        }
        public static void UnassignComputer(string deviceId)
        {
            Device device = RepositoryDevice.Get(deviceId, true);
            device.ParentComputerId = null;
            device.InUse = false;

            using (var db = new InventoryTIASPXContext())
            {
                db.Devices.AddOrUpdate(device);
                db.SaveChanges();

                db.Database.ExecuteSqlCommand($"UPDATE devices SET Computer_ComputerId = null WHERE deviceId LIKE \"{deviceId}\"");
            }
        }

        public static void Delete(string deviceId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(deviceId, true)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
                db.Devices.Add(device);
                db.SaveChanges();
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

        public static Device Get(string deviceId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.Where(x => x.DeviceId == deviceId).Include(x => x.ParentComputer).FirstOrDefault();
            }
        }

        public static List<Device> GetAllDevices()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Devices.OrderBy(x => x.Model).OrderBy(x => x.Brand).OrderBy(x => x.Type).ToList();
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

        public static void Delete(string deviceId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(deviceId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
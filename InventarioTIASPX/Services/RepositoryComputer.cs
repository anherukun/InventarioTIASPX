using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryComputer
    {
        public static void Add(Computer computer)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Computers.AddOrUpdate(computer);
                db.SaveChanges();
            }
        }

        public static Computer Get(string computerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Where(x => x.ComputerId == computerId)
                    .Include(x => x.Processor)
                    .Include(x => x.Devices)
                    .FirstOrDefault();
            }
        }

        public static List<Computer> GetAllComputers()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Include(x => x.Processor).ToList();
            }
        }

        public static List<string> GetAllDepartments()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Select(x => x.Department).OrderBy(x => x).ToList();
            }
        }

        public static void Delete(string computerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(computerId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryComputer
    {
        public static void Add(Computer computer)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Computers.Add(computer);
                db.SaveChanges();
            }
        }

        public static Computer Get(string computerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Where(x => x.ComputerId == computerId).Include(x => x.Processor).FirstOrDefault();
            }
        }

        public static List<Computer> GetAllComputers()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.ToList();
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
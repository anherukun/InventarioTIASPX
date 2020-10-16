﻿using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using WebGrease;

namespace InventarioTIASPX.Services
{
    public class RepositoryPrinter
    {
        public static void Add(Printer printer)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Printers.Add(printer);
                db.SaveChanges();
            }
        }
        public static void AddRange(List<Printer> printers)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Printers.AddRange(printers);
                db.SaveChanges();
            }
        }
        public static void Update(Printer printer)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Printers.AddOrUpdate(printer);
            }
        }

        public static Printer Get(string printerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Where(x => x.PrinterId == printerId).Include(x => x.User).FirstOrDefault();
            }
        }
        public static List<Printer> GetAll()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.OrderBy(x => x.Department).ToList();
            }
        }
        public static List<string> GetAllDepartments()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Select(x => x.Department).OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllModels()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Select(x => x.Model).OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllBrands()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Select(x => x.Brand).OrderBy(x => x).ToList();
            }
        }

        public static void RemoveUserFromAll(string userGUID)
        {
            List<Printer> printers = null;
            using (var db = new InventoryTIASPXContext())
            {
                printers = db.Printers.Where(x => x.UserGUID == userGUID).ToList();
            }

            if (printers != null)
                foreach (var item in printers)
                {
                    item.UserGUID = null;
                    item.User = null;

                    using (var db = new InventoryTIASPXContext())
                    {
                        db.Printers.AddOrUpdate(item);
                        db.SaveChanges();
                    }
                }
        }
        public static void Delete(string printerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(printerId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public static bool Exist(string printerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Any(x => x.PrinterId == printerId);
            }
        }
    }
}
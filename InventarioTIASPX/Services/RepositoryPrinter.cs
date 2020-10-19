using InventarioTIASPX.Models;
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
                db.SaveChanges();
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
                return db.Printers.Select(x => x.Department).Distinct().OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllModels()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Select(x => x.Model).Distinct().OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllBrands()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Printers.Select(x => x.Brand).Distinct().OrderBy(x => x).ToList();
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
            // SI EXISTEN FILEOBJECTS RELACIONEADOS CON LA ENTIDAD
            if (new RepositoryPrinterFiles().HasFilesRelated(printerId)) 
                // ROMPERAN LA RELACION QUE TIENEN CON LA ENTIDAD
                foreach (var item in new RepositoryPrinterFiles().GetAll(printerId))
                    new RepositoryGenericFiles().BreakRelationship(item.FileId);

            // SI EXISTEN NOTES RELACIONADOS CON LA ENTIDAD
            if (new RepositoryPrinterNotes().HasNotesRelated(printerId))
                // CADA NOTA SERA ELIMINADA
                foreach (var item in new RepositoryPrinterNotes().GetAll(printerId))
                    new RepositoryGenericNotes().Delete(item.NoteId);

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
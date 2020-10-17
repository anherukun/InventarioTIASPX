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

                RepositoryDevice.AssignComputer(computer.ComputerId, computer.ComputerId);
            }
        }

        public static void AddRange(List<Computer> computers)
        {
            try
            {
                using (var db = new InventoryTIASPXContext())
                {
                    db.Computers.AddRange(computers);
                    db.SaveChangesAsync();

                    foreach (var item in computers)
                    {
                        RepositoryDevice.AssignComputer(item.ComputerId, item.ComputerId);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
        }

        public static void Update(Computer computer)
        {
            try
            {
                using (var db = new InventoryTIASPXContext())
                {
                    db.Computers.AddOrUpdate(computer);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public static Computer Get(string computerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Where(x => x.ComputerId == computerId)
                    .Include(x => x.Processor)
                    .Include(x => x.Devices)
                    .Include(x => x.User)
                    .FirstOrDefault();
            }
        }

        public static List<Computer> GetAllComputers()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Include(x => x.Processor).OrderBy(x => x.Department).ToList();
            }
        }

        public static List<string> GetAllDepartments()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Select(x => x.Department).Distinct().OrderBy(x => x).ToList();
            }
        }
        public static List<string> GetAllLocations()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Select(x => x.Location).Distinct().OrderBy(x => x).ToList();
            }
        }

        public static void RemoveUserFromAll(string userGUID)
        {
            List<Computer> computers = null;
            using (var db = new InventoryTIASPXContext())
            {
                computers = db.Computers.Where(x => x.UserGUID == userGUID).ToList();
            }

            if (computers != null)
            {
                foreach (var item in computers)
                {
                    item.UserGUID = null;
                    item.User = null;
                    
                    using (var db = new InventoryTIASPXContext())
                    {
                        db.Computers.AddOrUpdate(item);
                        db.SaveChanges();
                    }
                }
            }
        }

        public static void Delete(string computerId)
        {
            // QUITA LA RELACION DE LA COMPUTADORA Y LOS ACCESORIOS
            Computer computer = RepositoryComputer.Get(computerId);
            if (computer.Devices != null)
                foreach (var item in computer.Devices)
                    RepositoryDevice.UnassignComputer(item.DeviceId);

            // COMPRUEBA QUE EXISTAN FILEOBJECTS RELACIONADOS CON LA ENTIDAD COMPUTADORA
            if (new RepositoryComputerFiles().HasFilesRelated(computerId))
            {
                // CADA FILEOBJECT QUE EXISTA
                foreach (var item in new RepositoryComputerFiles().GetAll(computerId))
                    // ROMPERA LA RELACION DE ESTA ENTIDAD CON LA ENTIDAD EXTERNA
                    new RepositoryGenericFiles().BreakRelationship(item.FileId);
            }
            // COMPRUEBA QUE EXISTAN NOTAS RELACIONADAS A LA ENTIDAD COMPUTADORA
            if (new RepositoryComputerNotes().HasNotesRelated(computerId))
            {
                // CADA NOTA QUE EXISTA
                foreach (var item in new RepositoryComputerNotes().GetAll(computerId))
                    // SERA ELIMINADA, PARA NO DEJAR UN LOG QUE NUNCA SE VA A OCUPAR
                    new RepositoryGenericNotes().Delete(item.NoteId);
            }

            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(computerId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public static bool Exist(string computerId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Computers.Any(x => x.ComputerId == computerId);
            }
        }
    }
}
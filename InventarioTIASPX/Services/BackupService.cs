using InventarioTIASPX.Models;
using InventarioTIASPX.Services.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class BackupService
    {
        // NOTES & FILES PREFIX: _COMP | _USR
        public static void RecoverData(Backup backup)
        {
            BackupService.ClearCurrentContext();

            List<Note> ComputerNotes = new List<Note>();
            List<Note> UserNotes = new List<Note>();

            // Inserta las entidades Primarias y Secundarias
            RepositoryDevice.AddRange(backup.Devices);
            RepositoryUser.AddRange(backup.Users);
            // RepositoryComputer.AddRange(backup.Computers);

            // Inserta una a una todas las entidades de Computers
            foreach (var item in backup.Computers)
            {
                RepositoryComputer.Add(item);
            }

            // Relaciona los dispositivos con las computadoras
            foreach (var item in backup.ComputerDeviceRelationship)
                RepositoryDevice.AssignComputer(item.FirstOrDefault().Key, item.FirstOrDefault().Value);

            // Reconstruye el origen de las notas y las inserta en la base de datos
            foreach (var item in backup.Notes)
            {
                if (item.ParentObjectId.Contains("COMP_"))
                {
                    item.ParentObjectId = item.ParentObjectId.Replace("COMP_", "");
                    new RepositoryComputerNotes().Add(item);
                }
                if (item.ParentObjectId.Contains("USR_"))
                {
                    item.ParentObjectId = item.ParentObjectId.Replace("USR_", "");
                    new RepositoryUserNotes().Add(item);
                }
            }                
        }

        /// <summary>
        /// Elimina las entidades del contexto actual
        /// </summary>
        private static void ClearCurrentContext()
        {
            // Elimina las entidades de Notas
            foreach (var item in new RepositoryGenericNotes().GetAll())
                new RepositoryGenericNotes().Delete(item.NoteId);

            // Elimina las entidades de Computadoras
            foreach (var item in RepositoryComputer.GetAllComputers())
                RepositoryComputer.Delete(item.ComputerId);

            // Elimina las entidades de Dispositivos
            foreach (var item in RepositoryDevice.GetAllDevices())
                RepositoryDevice.Delete(item.DeviceId);

            // Elimina las entidades de Usuarios
            foreach (var item in RepositoryUser.GetAllUsers())
                RepositoryUser.Delete(item.UserId);
        }
    }
}
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
        // NOTES & FILES PREFIX: _COMP | _USR | _PRT
        public static void RecoverData(Backup backup)
        {
            BackupService.ClearCurrentContext();

            // Inserta las entidades Primarias y Secundarias
            if (backup.Devices != null)
                RepositoryDevice.AddRange(backup.Devices);
            if (backup.Users != null)
                RepositoryUser.AddRange(backup.Users);
            if (backup.MemberOfs != null)
                RepositoryUserMemberOf.AddRange(backup.MemberOfs);
            if (backup.Printers != null)
                RepositoryPrinter.AddRange(backup.Printers);

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
                if (item.ParentObjectId.Contains("PRT_"))
                {
                    // TO-DO: IMPLEMENTAR REPOSITORIO DE NOTAS PARA IMPRESORAS
                    item.ParentObjectId = item.ParentObjectId.Replace("PRT_", "");
                    new RepositoryPrinterNotes().Add(item);
                }
            }

            foreach (var item in backup.Files)
            {
                if (item.ParentObjectId.Contains("COMP_"))
                {
                    item.ParentObjectId = item.ParentObjectId.Replace("COMP_", "");
                    new RepositoryComputerFiles().Add(item);
                }
                if (item.ParentObjectId.Contains("USR_"))
                {
                    item.ParentObjectId = item.ParentObjectId.Replace("USR_", "");
                    new RepositoryUserFiles().Add(item);
                }
                if (item.ParentObjectId.Contains("PRT_"))
                {
                    // TO-DO: IMPLEMENTAR REPOSITORIO DE ARCHVIOS PARA IMPRESORAS
                    item.ParentObjectId = item.ParentObjectId.Replace("PRT_", "");
                    new RepositoryPrinterFiles().Add(item);
                }
            }

            // Relaciona los usuarios con sus memberOfs
            foreach (var item in backup.UserMemberOfRelationship)
                RepositoryUserMemberOf.AssignUserToMemberOf(item.FirstOrDefault().Key, item.FirstOrDefault().Value);
        }

        /// <summary>
        /// Elimina las entidades del contexto actual
        /// </summary>
        private static void ClearCurrentContext()
        {
            // Elimina las entida de UserMemberOf
            foreach (var item in RepositoryUserMemberOf.GetAll(false))
                RepositoryUserMemberOf.Delete(item.UserMemberId);

            // Elimina las entidades de Fileobject
            foreach (var item in new RepositoryGenericFiles().GetAll())
                new RepositoryGenericFiles().Delete(item.FileId);

            // Elimina las entidades de Notas
            foreach (var item in new RepositoryGenericNotes().GetAll())
                new RepositoryGenericNotes().Delete(item.NoteId);

            // Elimina las entidades de Computadoras
            foreach (var item in RepositoryComputer.GetAllComputers())
                RepositoryComputer.Delete(item.ComputerId);

            // Elimina las entidades de las Impresoras
            foreach (var item in RepositoryPrinter.GetAll())
                RepositoryPrinter.Delete(item.PrinterId);

            // Elimina las entidades de Dispositivos
            foreach (var item in RepositoryDevice.GetAllDevices())
                RepositoryDevice.Delete(item.DeviceId);

            // Elimina las entidades de Usuarios
            foreach (var item in RepositoryUser.GetAllUsers())
                RepositoryUser.Delete(item.UserId);
        }
    }
}
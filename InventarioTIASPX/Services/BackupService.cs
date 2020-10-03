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
            List<Note> ComputerNotes = new List<Note>();
            List<Note> UserNotes = new List<Note>();

            // Inserta las entidades Primarias y Secundarias
            RepositoryDevice.AddRange(backup.Devices);
            RepositoryUser.AddRange(backup.Users);
            RepositoryComputer.AddRange(backup.Computers);

            // Reconstruye el origen de las notas
            foreach (var item in backup.Notes)
            {
                if (item.ParentObjectId.Contains("COMP_"))
                    ComputerNotes.Add(item);
                if (item.ParentObjectId.Contains("USR_"))
                    UserNotes.Add(item);
            }

            // Inserta las entidades dependientes
            foreach (var item in ComputerNotes) 
                new RepositoryComputerNotes().Add(item);
            foreach (var item in UserNotes)
                new RepositoryUserNotes().Add(item);
        }
    }
}
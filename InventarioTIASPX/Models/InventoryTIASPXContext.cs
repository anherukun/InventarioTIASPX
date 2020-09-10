using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Models
{
    public class InventoryTIASPXContext : DbContext
    {
#if DEBUG
        public InventoryTIASPXContext() : base("DefaultConnectionMySQL")
        {

        }
#else
        public InventoryTIASPXContext() : base("DefaultConnectionMySQL-PROD")
        {

        }
#endif
        static InventoryTIASPXContext()
        {
            DbConfiguration.SetConfiguration(new MySql.Data.Entity.MySqlEFConfiguration());
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Computer> Computers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FileObject> ComputersFiles { get; set; }
        public DbSet<Note> ComputerNotes { get; set; }
    }
}
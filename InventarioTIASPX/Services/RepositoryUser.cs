using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryUser
    {
        public static void Add(User user)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public static User Get(string userId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Users.Where(x => x.UserId == userId).FirstOrDefault();
            }
        }

        public static List<User> GetAllUsers()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Users.OrderBy(x => x.UserId).ToList();
            }
        }

        public static void Delete(string userId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(userId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
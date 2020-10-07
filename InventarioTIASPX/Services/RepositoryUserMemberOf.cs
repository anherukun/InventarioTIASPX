using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InventarioTIASPX.Services
{
    public class RepositoryUserMemberOf
    {
        public static void Add(UserMemberOf memberOf)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.UserMemberOfs.Add(memberOf);
                db.SaveChanges();
            }
        }

        public static UserMemberOf Get(string userMemberId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.UserMemberOfs.Where(x => x.UserMemberId == userMemberId).FirstOrDefault();
            }
        }

        public static List<UserMemberOf> GetAll()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.UserMemberOfs.ToList();
            }
        }

        public static List<UserMemberOf> GetAllByUser(string userId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                User u = RepositoryUser.Get(userId);
                return db.UserMemberOfs.Where(x => x.Users.Contains(u)).OrderBy(x => x.Description).ToList();
            }
        }

        public static void Delete(string userMemberId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(userMemberId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
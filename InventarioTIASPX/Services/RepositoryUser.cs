﻿using InventarioTIASPX.Controllers;
using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
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

        public static void AddRange(List<User> users)
        {
            try
            {
                using (var db = new InventoryTIASPXContext())
                {
                    db.Users.AddRange(users);
                    db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Update(User user)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Users.AddOrUpdate(user);
                db.SaveChanges();
            }
        }

        public static User Get(string userGUID)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Users.Where(x => x.UserGUID == userGUID).Include(x => x.MemberOfs).FirstOrDefault();
            }
        }
        public static User Get(long userId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Users.Where(x => x.UserId == userId).Include(x => x.MemberOfs).FirstOrDefault();
            }
        }

        public static List<User> GetAllUsers()
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Users.OrderBy(x => x.UserId).ToList();
            }
        }

        public static void Delete(long userId)
        {
            // TO-DO: METODO PARA DESASIGNAR LOS FILEOBJECTS DE LA ENTIDAD
            // TO-DO: METODO PARA DESASIGNAR LOS NOTES DE LA ENTIDAD
            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(userId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public static bool Exist(long userId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.Users.Any(x => x.UserId == userId);
            }
        }
    }
}
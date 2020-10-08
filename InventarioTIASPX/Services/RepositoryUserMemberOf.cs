using InventarioTIASPX.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public static void AddRange(List<UserMemberOf> memberOfs)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.UserMemberOfs.AddRange(memberOfs);
                db.SaveChanges();
            }
        }

        public static void AssignUserToMemberOf(string usermemberId, string userGUID)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Database.ExecuteSqlCommand($"INSERT INTO {db.Database.Connection.Database}.usermemberofusers VALUES (\"{usermemberId}\", \"{userGUID}\")");
                db.SaveChanges();
            }
        }

        public static void UnassignUserToMemberOf(string usermemberId, string userGUID)
        {
            using (var db = new InventoryTIASPXContext())
            {
                db.Database.ExecuteSqlCommand($"DELETE FROM {db.Database.Connection.Database}.usermemberofusers WHERE UserMemberOf_UserMemberId = \"{usermemberId}\" AND User_UserGUID = \"{userGUID}\"");
                db.SaveChanges();
            }
        }

        public static UserMemberOf Get(string userMemberId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.UserMemberOfs.Where(x => x.UserMemberId == userMemberId).Include(x => x.Users).FirstOrDefault();
            }
        }

        public static List<UserMemberOf> GetAll(bool includeChild)
        {
            using (var db = new InventoryTIASPXContext())
            {
                if (includeChild)
                    return db.UserMemberOfs.OrderBy(x => x.Description).Include(x => x.Users).ToList();

                return db.UserMemberOfs.OrderBy(x => x.Description).ToList();
            }
        }

        public static List<UserMemberOf> GetAllByUser(string userGUID)
        {
            User u = RepositoryUser.Get(userGUID);
            return u.MemberOfs;
            // using (var db = new InventoryTIASPXContext())
            // {
            //     
            //     return db.UserMemberOfs.Include(x => x.Users).Where(x => x.Users.Contains(u)).OrderBy(x => x.Description).ToList();
            // }
        }
        /// <summary>
        /// Elimina los registros de relacion que coincidan con el GUID del usuario
        /// </summary>
        /// <param name="userGUID">Parametro de identificacion de la entidad externa</param>
        public static void RemoveAllUserReference(string userGUID)
        {
            using (var db = new InventoryTIASPXContext())
            {
                // ELIMINA LOS REGISTROS QUE COINCIDAN CON USERGUID
                db.Database.ExecuteSqlCommand($"DELETE FROM {db.Database.Connection.Database}.usermemberofusers WHERE User_UserGUID = \"{userGUID}\"");
            }
        }
        /// <summary>
        /// Elimina los registros de relacion que coincidan con el ID de la entidad
        /// </summary>
        /// <param name="userMemberId">Parametro de identificaicon de la entidad</param>
        public static void RemoveAllMemberOfReference(string userMemberId)
        {
            using (var db = new InventoryTIASPXContext())
            {
                // ELIMINA LOS REGISTROS QUE COINCIDAN CON USERMEMBERID
                db.Database.ExecuteSqlCommand($"DELETE FROM {db.Database.Connection.Database}.usermemberofusers WHERE UserMemberOf_UserMemberId = \"{userMemberId}\"");
            }
        }
        /// <summary>
        /// Elimina la entidad y quita todas las relaciones con la misma
        /// </summary>
        /// <param name="userMemberId">Parametro de identificacion de la entidad</param>
        public static void Delete(string userMemberId)
        {
            RemoveAllMemberOfReference(userMemberId);

            using (var db = new InventoryTIASPXContext())
            {
                db.Entry(Get(userMemberId)).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public static bool Exist(string description)
        {
            using (var db = new InventoryTIASPXContext())
            {
                return db.UserMemberOfs.Any(x => x.Description == description);
            }
        }
    }
}
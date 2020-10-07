using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["users"] = RepositoryUser.GetAllUsers();
            return View();
        }

        [HttpGet]
        public ActionResult User(long? userId, string msgType, string msgString)
        {
            if (userId != null)
                if (RepositoryUser.Exist(userId.Value))
                {
                    if (msgType != null && msgString != null)
                    {
                        msgString = Application.ApplicationManager.Base64Decode(msgString);
                        ViewData["message"] = new { msgType, msgString };
                    }

                    User u = RepositoryUser.Get(userId.Value);
                    ViewData["user"] = u;
                    ViewData["notes"] = new RepositoryUserNotes().GetAll(u.UserGUID);
                    ViewData["files"] = new RepositoryUserFiles().GetAll(u.UserGUID);

                    ViewData["memberOfs"] = RepositoryUserMemberOf.GetAll(false);

                    return View();
                }

            return Redirect(Url.Action("", "Users"));
        }

        [HttpGet]
        public ActionResult NewUser(string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            return View();
        }

        [HttpGet]
        public ActionResult EditUser(long? userId, string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            if (userId != null)
                if (RepositoryUser.Exist(userId.Value))
                {
                    ViewData["user"] = RepositoryUser.Get(userId.Value);
                    return View();
                }

            return Redirect(Url.Action("", "Users"));
        }

        public ActionResult DeleteUser(long? userId, string msgType, string msgString)
        {
            if (userId != null)
            {
                if (RepositoryUser.Exist(userId.Value))
                {
                    if (msgType != null && msgString != null)
                    {
                        msgString = Application.ApplicationManager.Base64Decode(msgString);
                        ViewData["message"] = new { msgType, msgString };
                    }

                    ViewData["user"] = RepositoryUser.Get(userId.Value);

                    return View();
                }
            }

            return Redirect(Url.Action("", "Users"));
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            user.UserGUID = Application.ApplicationManager.GenerateGUID;

            if (!user.Email.Contains("@PEMEX.COM"))
                user.Email = $"{user.Email}@PEMEX.COM";

            if (RepositoryUser.Exist(user.UserId))
                return Redirect(Url.Action("NewUser", "Users", new { msgType = "error", msgString = Application.ApplicationManager.Base64Encode($"El usuario {user.UserId} ya se encuentra registrado.") }));
            else
                RepositoryUser.Add(user);

            return Redirect(Url.Action("NewUser", "Users", new { msgType = "success", msgString = Application.ApplicationManager.Base64Encode($"El usuario {user.UserId} fue registrado correctamente.") }));
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            try
            {
                user.UserGUID = RepositoryUser.Get(user.UserId).UserGUID;

                if (!user.Email.Contains("@PEMEX.COM"))
                    user.Email = $"{user.Email}@PEMEX.COM";

                RepositoryUser.Update(user);

                return Redirect(Url.Action("User", "Users", new { userId = user.UserId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode($"Los cambios se guardaron correctamente.") }));
            }
            catch (Exception ex)
            {
                return Redirect(Url.Action("User", "Users", new { userId = user.UserId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode($"{ex.Message}") }));
            }
        }

        [HttpPost]
        public ActionResult Delete(long userId)
        {
            User u = RepositoryUser.Get(userId);

            foreach (var item in u.MemberOfs)
            {
                RepositoryUserMemberOf.UnassignUserToMemberOf(item.UserMemberId, u.UserGUID);
            }
            RepositoryUser.Delete(userId);

            return Redirect(Url.Action("", "Users"));
        }
    }
}
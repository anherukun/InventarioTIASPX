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
                    ViewData["user"] = RepositoryUser.Get(userId.Value);

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
        public ActionResult EditUser(long userId, string msgType, string msgString)
        {
            if (msgType != null && msgString != null)
            {
                msgString = Application.ApplicationManager.Base64Decode(msgString);
                ViewData["message"] = new { msgType, msgString };
            }

            return View();
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
            return Redirect(Url.Action("User", "Users", new { userId = user.UserId }));
        }
    }
}
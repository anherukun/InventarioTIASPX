using InventarioTIASPX.Models;
using InventarioTIASPX.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventarioTIASPX.Controllers
{
    public class UserMemberOfController : Controller
    {
        // GET: UserMemberOf
        // public ActionResult Index()
        // {
        //     return View();
        // }
        [HttpPost]
        public ActionResult Add(UserMemberOf memberOf, string userGUID, string path, string controller)
        {
            if (userGUID != null)
            {
                if (memberOf != null)
                    if (memberOf.UserMemberId == "-1" && memberOf.Description != null)
                    {
                        if (memberOf.Description.Trim().Length > 0)
                        {
                            memberOf.UserMemberId = Application.ApplicationManager.GenerateGUID;
                            RepositoryUserMemberOf.Add(memberOf);
                            RepositoryUserMemberOf.AssignUserToMemberOf(memberOf.UserMemberId, userGUID);

                            return Redirect(Url.Action("User", "Users", new { userId = RepositoryUser.Get(userGUID).UserId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Los cambios se guardaron corractamente") }));
                        }

                    }
                    else if ((memberOf.UserMemberId != null && memberOf.UserMemberId != "-1"))
                    {
                        RepositoryUserMemberOf.AssignUserToMemberOf(memberOf.UserMemberId, userGUID);

                        return Redirect(Url.Action("User", "Users", new { userId = RepositoryUser.Get(userGUID).UserId, msgType = "success", msgString = Application.ApplicationManager.Base64Encode("Los cambios se guardaron corractamente") }));
                    }

                return Redirect(Url.Action("User", "Users", new { userId = RepositoryUser.Get(userGUID).UserId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode("No se pudo completar la solicitud") }));
            }

            return Redirect(Url.Action("", "Users", new { userId = RepositoryUser.Get(userGUID).UserId, msgType = "error", msgString = Application.ApplicationManager.Base64Encode("No se pudo completar la solicitud") }));
        }

        [HttpGet]
        public ActionResult Remove(string memberID, string userGUID, string redirect)
        {
            RepositoryUserMemberOf.UnassignUserToMemberOf(memberID, userGUID);

            return Redirect(redirect);
        }
    }
}
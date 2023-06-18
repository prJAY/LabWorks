using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Lab_Works.Models;
using System;

namespace Lab_Works.Controllers
{
    [RoutePrefix("Home")]
    public class HomeController : Controller
    {
        [Route("~/")]
        public ActionResult Login() => View();

        [HttpPost]
        [Route("Authenticate")]
        public ActionResult Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.msgclass = "text-danger";
                ViewBag.msg = "Please fill out all the details";
                return View("Login");
            }
            else
            {
                CustomContext customContext = new CustomContext();
                User user = customContext.Users.SingleOrDefault(u => u.user_id == username && u.user_password == password);
                if (user != null && user.user_password == password)
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    string ctrl;
                    switch (user.user_type)
                    {
                        case "A": ctrl = "Admin"; break;
                        case "T": ctrl = "Technician"; break;
                        case "F": ctrl = "Faculty"; break;
                        default: ctrl = "Student"; break;
                    }
                    Session["user_id"] = user.user_id;
                    Session["user_name"] = user.user_fname;
                    Session["user_ctrl"] = ctrl;
                    return RedirectToAction("Index",CustomFunctions.ctrltype());
                }
                else
                {
                    ViewBag.msgclass = "text-danger";
                    ViewBag.msg = "Please enter valid username/password";
                    return View("Login");
                }
            }
        }
        
        [Route("Logout")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["user_id"] = null;
            Session["user_name"] = null;
            ViewBag.msgclass = "text-success";
            ViewBag.msg = "You have successfully logged out";
            return View("Login");
        }

        [Authorize]
        public ActionResult Index()
        {
            List<MenuGroup> menuGroups = CustomFunctions.LoadMenuGroup();
            return View(menuGroups);
        }
        
        [Authorize]
        [Route("Profile")]
        public ActionResult UpdateProfile()
        {
            CustomContext customContext = new CustomContext();
            string user_id = Session["user_id"] as string;
            string ctrl = Session["user_ctrl"] as string;
            User user = customContext.Users.SingleOrDefault(u => u.user_id == user_id);
            if (TryUpdateModel(user, new string[] { "user_password" }))
            {
                customContext.SaveChanges();
            }
            return View("UserProfile", user);
        }
    }
}
using Lab_Works.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;

namespace Lab_Works.Controllers
{
    [Authorize(Roles = "A")]
    public class UserController : Controller
    {
        private readonly CustomContext customContext = new CustomContext();
        static List<User> users = new List<User>();

        public ActionResult Index()
        {
            List<User> users = customContext.Users.OrderBy(u => u.user_id).ThenBy(u => u.user_type).ToList();
            return View(users);
        }

        public ActionResult Add() => View();

        public ActionResult OpenFile(HttpPostedFileBase xlsfile)
        {
            if (xlsfile == null)
            {
                TempData["msg"] = "Please select a .xlsx file and try again.";
                return RedirectToAction("Add");
            }
            try
            {
                string filedir = Server.MapPath("/Files/Uploads/");
                string filepath = Path.Combine(filedir, Path.GetFileName(xlsfile.FileName));
                Directory.CreateDirectory(filedir);
                xlsfile.SaveAs(filepath);

                using (var stream = System.IO.File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        users.Clear();
                        do
                        {
                            while (reader.Read())
                            {
                                User u = new User();
                                u.user_id = reader.GetValue(0).ToString();
                                u.user_password = reader.GetValue(1).ToString();
                                u.user_fname = reader.GetString(2);
                                u.user_lname = reader.GetString(3);
                                u.user_email = reader.GetString(4);
                                u.user_type = reader.GetString(5);
                                string types = "AFTS";
                                if (!types.Contains(u.user_type))
                                {
                                    u.user_type = "S";
                                }
                                users.Add(u);
                            }
                        } while (reader.NextResult());
                        users.RemoveAt(0);
                    }
                }
            }
            catch
            {
                TempData["msg"] = "Could not load the file. Try again later.";
                return View("Add");
            }
            return View("Add", users);
        }
        public ActionResult SaveFile()
        {
            List<User> failedusers = new List<User>();
            if (users.Count() > 0)
            {
                int count = 0;
                foreach (User u in users)
                {
                    try
                    {
                        customContext.Users.Add(u);
                        customContext.SaveChanges();
                        count++;
                    }
                    catch
                    {
                        failedusers.Add(u);
                    }
                }
                if (failedusers.Count() > 0)
                {
                    ViewBag.msg = "Failed";
                }
                else
                {
                    ViewBag.msg = count + " Users have been added successfully";
                }
            }
            return View("Add", failedusers);
        }
        public ActionResult Details(string id = "")
        {
            User user = customContext.Users.SingleOrDefault(u => u.user_id.Equals(id));
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index","User",CustomFunctions.ctrltype());
            }
        }
        public ActionResult Update(string user_id = "")
        {
            User user = customContext.Users.SingleOrDefault(u => u.user_id.Equals(user_id));
            if (TryUpdateModel(user))
            {
                customContext.SaveChanges();
                TempData["msg"] = "User data updated";
                return RedirectToAction("Index", "User", CustomFunctions.ctrltype());
            }
            else
            {
                return View("Details", user);
            }
            
        }
        public ActionResult Delete(string user_id = "")
        {
            User user = customContext.Users.SingleOrDefault(u => u.user_id.Equals(user_id));
            if (user != null)
            {
                customContext.Users.Remove(user);
                customContext.SaveChanges();
                TempData["msg"] = "User has been deleted";
            }
            else
            {
                TempData["msg"] = "Something went wrong. Operation Failed !";
            }
            return RedirectToAction("Index", "User", CustomFunctions.ctrltype());
        }
    }
}
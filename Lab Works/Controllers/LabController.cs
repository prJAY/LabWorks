using Lab_Works.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_Works.Controllers
{
    public class LabController : Controller
    {
        private readonly CustomContext customContext = new CustomContext();

        [Authorize(Roles = "T,F,S")]
        public ActionResult Index()
        {
            List<Lab> lab = customContext.Labs.ToList();
            return View(lab);
        }

        [Authorize(Roles = "T")]
        public ActionResult Create() => View();

        [Authorize(Roles = "T")]
        public ActionResult PostCreate(Lab lab)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customContext.Labs.Add(lab);
                    customContext.SaveChanges();
                    TempData["msg"] = "Lab has been created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["msg"] = "Something went wrong. Operation Failed !";
                }
            }
            return View("Create");
        }

        [Authorize(Roles = "T")]
        public ActionResult Details(string id = "")
        {
            Lab lab = customContext.Labs.SingleOrDefault(l => l.lab_id == id);
            if (lab != null)
            {
                return View(lab);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "T")]
        public ActionResult Update(string lab_id)
        {
            if (!string.IsNullOrEmpty(lab_id))
            {
                Lab lab = customContext.Labs.SingleOrDefault(l => l.lab_id == lab_id);
                if (lab != null && TryUpdateModel(lab))
                {
                    customContext.SaveChanges();
                    TempData["msg"] = "Lab has been Updated";
                }
                else
                {
                    TempData["msg"] = "Something went wrong. Operation Failed !";
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "T")]
        public ActionResult Delete(string lab_id)
        {
            if (!string.IsNullOrEmpty(lab_id))
            {
                Lab lab = customContext.Labs.SingleOrDefault(l => l.lab_id == lab_id);
                if (lab != null)
                {
                    customContext.Labs.Remove(lab);
                    customContext.SaveChanges();
                    TempData["msg"] = "Lab has been deleted";
                }
                else
                {
                    TempData["msg"] = "Something went wrong. Operation Failed !";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
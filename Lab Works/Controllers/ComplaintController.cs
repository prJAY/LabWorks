using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab_Works.Models;

namespace Lab_Works.Controllers
{
    [Authorize(Roles = "T,F,S")]
    public class ComplaintController : Controller
    {
        private readonly CustomContext customContext = new CustomContext();
        static int idForUpdate = 0;

        public ActionResult Index()
        {
            string userid = Session["user_id"] as string;
            string ctrl = Session["user_ctrl"] as string;
            List<Complaint> complaints = new List<Complaint>();
            if (ctrl.Equals("Technician"))
            {
                complaints = customContext.Complaints.Where(c => c.comp_status.Equals("Pending") || c.comp_status.Equals("Processing")).ToList();
            }
            else if (ctrl.Equals("Faculty"))
            {
                //if incharge of the lab
                Lab lab = customContext.Labs.FirstOrDefault(l => l.lab_incharge_id.Equals(userid));
                if (lab != null)
                    complaints = customContext.Complaints.Where(c => c.comp_lab_id.Equals(lab.lab_id)).ToList();
            }
            if(complaints != null)
            {
                return View(complaints);
            }
            else
            {
                return RedirectToAction("History", "Complaint", CustomFunctions.ctrltype());
            }
            
        }
        public ActionResult Raise() => View();
        public ActionResult PostRaise()
        {
            Complaint complaint = new Complaint();
            if (TryUpdateModel(complaint, new string[] { "comp_lab_id", "comp_equip_no", "comp_equip_type", "comp_details" }))
            {
                complaint.comp_raised_by_id = Session["user_id"] as string;
                complaint.comp_solved_by_id = "none";
                complaint.comp_raised_date = DateTime.Now;
                complaint.comp_status = "Pending";

                customContext.Complaints.Add(complaint);
                customContext.SaveChanges();
                TempData["msg"] = "Complaint has been registered";
                return RedirectToAction("History");
            }
            else
            {
                return View("Raise");
            }
        }
        public ActionResult History()
        {
            string userid = Session["user_id"] as string;
            string ctrl = Session["user_ctrl"] as string;
            List<Complaint> complaints = new List<Complaint>();
            if (ctrl.Equals("Technician"))
            {
                complaints = customContext.Complaints.Where(c => c.comp_status.Equals("Resolved") || c.comp_status.Equals("Cancelled")).ToList();
            }
            else
            {
                complaints = customContext.Complaints.Where(c => c.comp_raised_by_id == userid).ToList();
            }
            return View("Index",complaints);
        }
        public ActionResult Details(int id = 0)
        {
            string userid = Session["user_id"] as string;
            string ctrl = Session["user_ctrl"] as string;
            Complaint complaint = new Complaint();
            if (ctrl.Equals("Technician") || CustomFunctions.CheckIncharge())
            {
                complaint = customContext.Complaints.SingleOrDefault(c => c.comp_id == id);
            }
            else
            {
                complaint = customContext.Complaints.SingleOrDefault(c => c.comp_id == id && c.comp_raised_by_id == userid);
            }
            if (complaint != null)
            {
                idForUpdate = id;
                return View(complaint);
            }
            else
            {
                return RedirectToAction("History");
            }
        }
        public ActionResult StatusUpdate(string comp_status)
        {
            try
            {
                string[] possiblestatus;
                if(Session["user_ctrl"] as string == "Technician")
                {
                    possiblestatus = new string[] { "Pending", "Processing", "Resolved", "Cancelled" };
                }
                else
                {
                    possiblestatus = new string[] { "Pending", "Cancelled" };
                }
                CustomContext customContext = new CustomContext();
                Complaint complaint = customContext.Complaints.SingleOrDefault(c => c.comp_id == idForUpdate);
                if (complaint != null && comp_status != null && possiblestatus.Contains(comp_status,StringComparer.CurrentCultureIgnoreCase))
                {
                    if (comp_status.Equals("Resolved"))
                    {
                        complaint.comp_solved_by_id = Session["user_id"] as string;
                    }
                    complaint.comp_status = comp_status;
                    customContext.SaveChanges();
                    TempData["msg"] = "Complaint status set to "+comp_status;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction("History");
        }
    }
}
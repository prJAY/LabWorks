using Lab_Works.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab_Works.Controllers
{
    public class RegistrarController : Controller
    {
        private readonly CustomContext customContext = new CustomContext();

        [Authorize(Roles = "F")]
        public ActionResult LabSession()
        {
            string uid = Session["user_id"] as string;
            List<LabSession> labSessions = customContext.LabSessions.Where(ls => ls.sess_creator_id.Equals(uid)).ToList();
            return View(labSessions);
        }

        [Authorize(Roles = "F")]
        public ActionResult CreateSession()
        {
            string uid = Session["user_id"] as string;
            LabSession labSession = new LabSession();
            if (TryUpdateModel(labSession,new string[] { "sess_class","sess_lab_id","sess_end_time" }))
            {
                labSession.sess_creator_id = uid;
                customContext.LabSessions.Add(labSession);
                customContext.SaveChanges();
                TempData["msg"] = "Session Created";
            }
            else
            {
                TempData["msg"] = "Something went wrong. Operation Failed !";
            }
            return RedirectToAction("LabSession");
        }

        [Authorize(Roles = "F")]
        public ActionResult Students(int id = 0)
        {
            LabSession ls = customContext.LabSessions.FirstOrDefault(l => l.sess_id == id);
            List<LabRegistrar> labRegistrars = customContext.LabRegistrars.Where(lr => lr.reg_sess_id == id).ToList();
            List<SessionRegistrar> SRList = new List<SessionRegistrar>();
            string creatorname = CustomFunctions.GetUserName(ls.sess_creator_id);
            foreach (LabRegistrar lr in labRegistrars)
            {
                SessionRegistrar sr = new SessionRegistrar();
                sr.sr_user_id = lr.reg_user_id;
                sr.sr_sess_id = lr.reg_sess_id;
                sr.sr_creator = creatorname;
                sr.sr_lab_id = ls.sess_lab_id;
                sr.sr_pc_no = lr.reg_pc_no;
                sr.sr_end_time = ls.sess_end_time;
                SRList.Add(sr);
            }
            return View("Index",SRList);
        }

        [Authorize(Roles = "S")]
        public ActionResult Index()
        {
            string uid = Session["user_id"] as string;
            List<LabRegistrar> labRegistrars = customContext.LabRegistrars.Where(lr => lr.reg_user_id.Equals(uid)).ToList();

            List<SessionRegistrar> SRList = new List<SessionRegistrar>();
            foreach(LabRegistrar lr in labRegistrars)
            {
                LabSession ls = customContext.LabSessions.FirstOrDefault(l => l.sess_id == lr.reg_sess_id);

                SessionRegistrar sr = new SessionRegistrar();
                sr.sr_user_id = lr.reg_user_id;
                sr.sr_sess_id = lr.reg_sess_id;
                sr.sr_creator = CustomFunctions.GetUserName(ls.sess_creator_id);
                sr.sr_lab_id = ls.sess_lab_id;
                sr.sr_pc_no = lr.reg_pc_no;
                sr.sr_end_time = ls.sess_end_time;
                SRList.Add(sr);
            }
            return View(SRList);
        }

        [Authorize(Roles = "S")]
        public ActionResult Create()
        {
            string uid = Session["user_id"] as string;
            LabRegistrar labRegistrar = new LabRegistrar();
            if(TryUpdateModel(labRegistrar,new string[] { "reg_sess_id" , "reg_pc_no"}))
            {
                try
                {
                    LabSession labSession = customContext.LabSessions.FirstOrDefault(ls => ls.sess_id == labRegistrar.reg_sess_id && ls.sess_end_time > DateTime.Now);
                    if (labSession != null)
                    {
                        labRegistrar.reg_id = labSession.sess_id + "-" + uid;
                        labRegistrar.reg_user_id = uid;
                        customContext.LabRegistrars.Add(labRegistrar);
                        customContext.SaveChanges();
                        TempData["msg"] = "Lab Entry Created";
                    }
                }
                catch
                {
                    TempData["msg"] = "Something went wrong. Operation Failed !";
                }
            }
            return RedirectToAction("Index");
        }
    }
}
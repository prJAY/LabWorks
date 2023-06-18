using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Lab_Works.Models;
using System.Web.Security;
using System.Web.Routing;
using System.Web.Mvc;

namespace Lab_Works
{
    public class CustomFunctions
    {
        public static string GetConnection()
        {
            string str = ConfigurationManager.ConnectionStrings["CustomContext"].ConnectionString;
            return str;
        }
        public static string GetUserIP()
        {
            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public static string GetUserName(string user_id)
        {
            CustomContext customContext = new CustomContext();
            User user = customContext.Users.SingleOrDefault(u => u.user_id == user_id);
            string username = user.user_fname + " " + user.user_lname;
            return username;
        }
        public static string GetUserType()
        {
            return HttpContext.Current.Session["user_ctrl"] as string;
        }
        public static bool CheckIncharge()
        {
            string uid = HttpContext.Current.Session["user_id"] as string;
            CustomContext customContext = new CustomContext();
            Lab lab = customContext.Labs.FirstOrDefault(l => l.lab_incharge_id.Equals(uid));
            if (lab == null)
                return false;
            else
                return true;
        }

        public static RouteValueDictionary ctrltype()
        {
            RouteValueDictionary valuePair = new RouteValueDictionary { { "usertype", HttpContext.Current.Session["user_ctrl"] as string } };
            return valuePair;
        }
        public static bool sendEmail(string user_id)
        {
            return true;
        }
        public static List<SelectListItem> GetSessionList()
        {
            CustomContext customContext = new CustomContext();
            List<LabSession> labSessions = customContext.LabSessions.Where(ls => ls.sess_end_time > DateTime.Now).ToList();
            
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (LabSession session in labSessions)
            {
                SelectListItem item = new SelectListItem();
                item.Text = session.sess_id + " " + GetUserName(session.sess_creator_id);
                item.Value = session.sess_id.ToString();
                selectListItems.Add(item);
            }
            return selectListItems;
        }
        public static List<SelectListItem> GetLabList()
        {
            CustomContext customContext = new CustomContext();
            List<Lab> labs = customContext.Labs.ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Lab lab in labs)
            {
                SelectListItem item = new SelectListItem();
                item.Text = lab.lab_id;
                item.Value = lab.lab_id;
                selectListItems.Add(item);
            }
            return selectListItems;
        }
        public static List<SelectListItem> GetComplaintStatusList()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            if(HttpContext.Current.Session["user_ctrl"] as string == "Technician")
            {
                selectListItems.Add(new SelectListItem { Text = "Processing", Value = "Processing" });
                selectListItems.Add(new SelectListItem { Text = "Resolved", Value = "Resolved" });
            }
            selectListItems.Add(new SelectListItem { Text = "Pending", Value = "Pending" });
            selectListItems.Add(new SelectListItem { Text = "Cancelled", Value = "Cancelled" });

            return selectListItems;
        }
        public static List<SelectListItem> GetFacultyList()
        {
            CustomContext customContext = new CustomContext();
            List<User> users = customContext.Users.Where(u => u.user_type == "F").ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (User user in users)
            {
                SelectListItem item = new SelectListItem();
                item.Text = user.user_fname + " " + user.user_lname;
                item.Value = user.user_id;
                selectListItems.Add(item);
            }
            return selectListItems;
        }

        public static List<MenuGroup> LoadMenuGroup()
        {
            string ctrl = HttpContext.Current.Session["user_ctrl"] as string;
            //Users -Admin
            MenuItem mi_u1 = new MenuItem()
            {
                ItemName = "Add",
                ItemLink = "/" + ctrl + "/User/Add"
            };
            MenuItem mi_u2 = new MenuItem()
            {
                ItemName = "View",
                ItemLink = "/" + ctrl + "/User"
            };

            //Labs
            MenuItem mi_l1 = new MenuItem()
            {
                ItemName = "Create",
                ItemLink = "/" + ctrl + "/Lab/Create"
            };
            MenuItem mi_l2 = new MenuItem()
            {
                ItemName = "View",
                ItemLink = "/" + ctrl + "/Lab"
            };

            //Complaint
            MenuItem mi_c1 = new MenuItem()
            {
                ItemName = "Raise",
                ItemLink = "/" + ctrl + "/Complaint/Raise"
            };
            MenuItem mi_c2 = new MenuItem()
            {
                ItemName = "View",
                ItemLink = "/" + ctrl + "/Complaint/History"
            };

            //Complaint - Technician & Lab Inchage
            MenuItem mi_c3 = new MenuItem()
            {
                ItemName = "Active",
                ItemLink = "/" + ctrl + "/Complaint"
            };

            //Registrar
            MenuItem mi_r1 = new MenuItem()
            {
                ItemName = "Create Session",
                ItemLink = "/" + ctrl + "/Registrar/LabSession"
            };

            //Registrar - Student
            MenuItem mi_r2 = new MenuItem()
            {
                ItemName = "Lab Registrar",
                ItemLink = "/" + ctrl + "/Registrar"
            };

            List<MenuGroup> MenuList = new List<MenuGroup>();

            switch (ctrl)
            {
                case "Admin":
                    MenuGroup MenuAdmin = new MenuGroup();
                    MenuAdmin.GroupName = "Users";
                    MenuAdmin.MenuItems = new List<MenuItem>() { mi_u1, mi_u2 };
                    MenuList.Add(MenuAdmin);
                    break;

                case "Technician":
                    MenuGroup MenuTech1 = new MenuGroup();
                    MenuTech1.GroupName = "Labs";
                    MenuTech1.MenuItems = new List<MenuItem>() { mi_l1, mi_l2 };

                    MenuGroup MenuTech2 = new MenuGroup();
                    MenuTech2.GroupName = "Complaints";
                    MenuTech2.MenuItems = new List<MenuItem>() { mi_c3, mi_c2 };

                    MenuList.Add(MenuTech1);
                    MenuList.Add(MenuTech2);
                    break;

                case "Faculty":
                    MenuGroup MenuFaculty1 = new MenuGroup();
                    MenuFaculty1.GroupName = "Complaints";
                    if (CheckIncharge())
                    {
                        MenuFaculty1.MenuItems = new List<MenuItem>() { mi_c1, mi_c2 , mi_c3 };
                    }
                    else
                    {
                        MenuFaculty1.MenuItems = new List<MenuItem>() { mi_c1, mi_c2 };
                    }

                    MenuGroup MenuFaculty2 = new MenuGroup();
                    MenuFaculty2.GroupName = "Lab Sessions";
                    MenuFaculty2.MenuItems = new List<MenuItem>() { mi_r1, mi_l2 };

                    MenuList.Add(MenuFaculty1);
                    MenuList.Add(MenuFaculty2);
                    break;

                case "Student":
                    MenuGroup MenuStudent1 = new MenuGroup();
                    MenuStudent1.GroupName = "Labs";
                    MenuStudent1.MenuItems = new List<MenuItem>() { mi_r2, mi_l2 };

                    MenuGroup MenuStudent2 = new MenuGroup();
                    MenuStudent2.GroupName = "Complaints";
                    MenuStudent2.MenuItems = new List<MenuItem>() { mi_c1, mi_c2 };

                    MenuList.Add(MenuStudent1);
                    MenuList.Add(MenuStudent2);
                    break;
            }
            return MenuList;
        }
    }
}
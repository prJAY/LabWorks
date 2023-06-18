using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lab_Works.Models
{
    public class MenuItem
    {
        public string ItemName { get; set; }
        public string ItemLink { get; set; }
    }
    public class MenuGroup
    {
        public string GroupName { get; set; }
        public List<MenuItem> MenuItems { get; set; }
    }
}
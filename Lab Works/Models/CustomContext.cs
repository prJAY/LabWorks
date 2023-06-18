using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Lab_Works.Models
{
    public class CustomContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Lab> Labs { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<LabRegistrar> LabRegistrars { get; set; }
        public DbSet<LabSession> LabSessions { get; set; }
    }
}
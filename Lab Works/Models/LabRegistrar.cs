using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab_Works.Models
{
    [Table("Session_Master")]
    public class LabSession
    {
        [Key]
        public int sess_id { get; set; }
        [Required]
        public string sess_class { get; set; }
        public string sess_creator_id { get; set; }
        [Required]
        public string sess_lab_id { get; set; }
        [Required]
        public DateTime sess_end_time { get; set; }
    }

    [Table("Lab_Registrar")]
    public class LabRegistrar
    {
        [Key]
        public string reg_id { get; set; }
        public string reg_user_id { get; set; }
        [Required]
        public int reg_sess_id { get; set; }
        [Required]
        public string reg_pc_no { get; set; }
    }

    /// <summary>
    /// For showing Session  & Registrar Data to Users Only
    /// Not to do anything with database
    /// </summary>
    public class SessionRegistrar
    {
        public string sr_user_id { get; set; }
        public int sr_sess_id { get; set; }
        public string sr_creator { get; set; }
        public string sr_lab_id { get; set; }
        public string sr_pc_no { get; set; }
        public DateTime sr_end_time { get; set; }
    }
}
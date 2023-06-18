using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab_Works.Models
{
    [Table("Complaint_Master")]
    public class Complaint
    {
        [Key]
        public int comp_id { get; set; }
        [Required]
        public string comp_lab_id { get; set; }
        [Required]
        public string comp_equip_no { get; set; }
        [Required]
        public string comp_equip_type { get; set; }
        [Required]
        public string comp_details { get; set; }
        public string comp_raised_by_id { get; set; }
        public string comp_solved_by_id { get; set; }
        public DateTime comp_raised_date { get; set; }
        public string comp_status { get; set; }
    }
}
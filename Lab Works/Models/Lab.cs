using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab_Works.Models
{
    [Table("Lab_Master")]
    public class Lab
    {
        [Required][Key]
        public string lab_id { get; set; }
        [Required]
        public string lab_name { get; set; }
        [Required]
        public string lab_incharge_id { get; set; }
        [Required]
        public int no_of_pc { get; set; }
        [Required]
        public string starting_ip { get; set; }
        [Required]
        public string ending_ip { get; set; }
    }
}
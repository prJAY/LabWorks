using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab_Works.Models
{
    [Table("User_Master")]
    public class User
    {
        [Required][Key]
        public string user_id { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must be within 6 - 12 characters")]
        public string user_password { get; set; }
        [Required]
        public string user_fname { get; set; }
        [Required]
        public string user_lname { get; set; }
        [Required]
        public string user_email { get; set; }
        [Required]
        public string user_type { get; set; }
    }
}
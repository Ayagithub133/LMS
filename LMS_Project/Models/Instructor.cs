using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LMS_Project.Models
{
    public class Instructor
    {
        [Key]
        [Required]
        public int InsId { set; get; }

        [Required]
        [MaxLength(50)]
        public string FName { set; get; }

        [Required]
        [MaxLength(50)]
        public string LName { set; get; }

        [Required]
        public string InsImage { set; get; }

        [Required]
        [MaxLength(20)]
        public string Password { set; get; }

        [Required]
        [MaxLength(300)]
        [EmailAddress]
        public string Email { set; get; }
        [Url]
        public string Url { set; get; }

        public string IntrVideo { set; get; }

        public string Experties { set; get; }
        public bool Online { set; get; }

        public List<Course> courses{set;get;}

    }
}
using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public static class LoginViewModel
    {
        [Key]
        [Required]
        public static int InsId { set; get; }



        [Required]
        [MaxLength(300)]
        [EmailAddress]
        public static string Email { set; get; }
        public static string FName { set; get; }
        public static string LName { set; get; }
        public static string Image { set; get; }
        

        public static void LoginView(Instructor instructor)
        {
            InsId = instructor.InsId;
          
            Email = instructor.Email;
            FName = instructor.FName;
            LName = instructor.LName;
            Image = instructor.InsImage;

        }
    }
}
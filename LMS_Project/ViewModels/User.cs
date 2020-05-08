using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public class User
    {
        public List<Instructor> Instructors { set; get; }
        //List<Student> Students { set;get }
    }
}
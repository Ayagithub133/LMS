using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.Models
{
    public class Category
    {
        public int CategoryId { set; get; }
        public string CategoryName { set; get; }
        public string Description { set; get; }
        public string CategoryImage { set; get; }
        public List<Course> Courses { set; get; }
    }
}
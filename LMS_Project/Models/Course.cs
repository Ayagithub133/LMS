using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.Models
{
    public class Course
    {
        public int Id { set; get; }
        public string CourseDescription { set; get; }
        public string CourseTitle { set; get; }
        public string CourseImage { set; get; }
        public Level Level { set; get; }
        public decimal Duration { set; get; }
        public decimal Price { set; get; }
        public DateTime StartCourse { set; get; }
        public DateTime EndCourse { set; get; }
        public int InsId { set; get; }
        public int CategoryId { set; get; }
        public Category category { set; get; }
        public Instructor instructor { set; get; }
        public List<Lesson> Lessons { set; get; }
    }
    public enum Level
    {Beginner = 0,InterMediate=1,Advanced=2}
}
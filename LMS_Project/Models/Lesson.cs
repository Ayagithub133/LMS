using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS_Project.Models
{
    public class Lesson
    {
      public int LessonId { set; get; } 
      public string LessonTitle { set; get; }
      public string video { set; get; }
      public int  VideoSize { set; get; }
      public string TextContent { set; get; }
      public int TextContentSize { set; get; }
        public string LessonImage { set; get; }
      public int ID { set; get; }
      public Course course { set; get; }

    }
}

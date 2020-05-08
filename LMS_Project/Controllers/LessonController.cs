using LMS_Project.DAL;
using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    public class LessonController : Controller
    {
        Lesson_DAL lesson_DAL = new Lesson_DAL();
        Course_DAL course_DAL = new Course_DAL();
        List<SelectListItem> selectListCourses = new List<SelectListItem>();
        // GET: Lesson
        [HttpGet]
        public ActionResult AddLesson()
        {
           
            foreach (var item in course_DAL.AllCourses())
            {
                selectListCourses.Add(new SelectListItem() { Text = item.CourseTitle, Value = item.Id.ToString() });
            }

            ViewBag.CoursesList = selectListCourses;
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddLesson(Lesson lesson , HttpPostedFileBase[] upload)
        {
            string imagePath = Path.Combine(Server.MapPath("~/Upload/"), upload[0].FileName);
            upload[0].SaveAs(imagePath);
            lesson.LessonImage = upload[0].FileName;

            string videoPath = Path.Combine(Server.MapPath("~/Videos/"), upload[1].FileName);
            upload[1].SaveAs(videoPath);
            lesson.video = upload[1].FileName;
            lesson.VideoSize = upload[1].ContentLength / 1024;

            string textPath = Path.Combine(Server.MapPath("~/TextFiles/"), upload[2].FileName);
            upload[2].SaveAs(textPath);
            lesson.TextContent = upload[2].FileName;
            lesson.TextContentSize = upload[2].ContentLength / 1024;
            lesson_DAL.AddLesson(lesson);
            foreach (var item in course_DAL.AllCourses())
            {
                selectListCourses.Add(new SelectListItem() { Text = item.CourseTitle, Value = item.Id.ToString() });
            }

            ViewBag.CoursesList = selectListCourses;
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }

        //------------lessons of course--------
        [HttpGet]
       
        public ActionResult getLessonsOfCourse(int id ,int index = 0)
        {
            Session["CourseId"] = id;
            float pages = 0.0f;


            pages = lesson_DAL.countOfLessons(id) / 12.0f;
            if (pages != 0.0f)
            {
                if ((pages - (int)pages) < 1)
                {
                    ViewBag.pageNumber = (int)pages + 1;

                }
            }
            else
            {

                ViewBag.pageNumber = pages;
            }
            
            ViewBag.Lessons = lesson_DAL.LessonesOfOneCourse(id, index);


            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }
        //------------Edit Lesson------------------
        [HttpGet]
        public ActionResult EditLesson(int id)
        {
            Lesson lesson = lesson_DAL.GetLessonById(id);
            ViewBag.Lesson = lesson.LessonTitle;
            foreach (var item in course_DAL.AllCourses())
            {
                selectListCourses.Add(new SelectListItem() { Text = item.CourseTitle, Value = item.Id.ToString() });
            }

            ViewBag.CoursesList = selectListCourses;
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View(lesson);
            }
        }
        [HttpPost]
        public ActionResult EditLesson(Lesson lesson, HttpPostedFileBase[] upload)
        {

            string imagePath = Path.Combine(Server.MapPath("~/Upload/"), upload[0].FileName);
            upload[0].SaveAs(imagePath);
            lesson.LessonImage = upload[0].FileName;

            string videoPath = Path.Combine(Server.MapPath("~/Videos/"), upload[1].FileName);
            upload[1].SaveAs(videoPath);
            lesson.video = upload[1].FileName;
            lesson.VideoSize = upload[1].ContentLength / 1024;

            string textPath = Path.Combine(Server.MapPath("~/TextFiles/"), upload[2].FileName);
            upload[2].SaveAs(textPath);
            lesson.TextContent = upload[2].FileName;
            lesson.TextContentSize = upload[2].ContentLength / 1024;
            

            lesson_DAL.EditLesson(lesson);
            ViewBag.Lessons = lesson_DAL.LessonesOfOneCourse(lesson.ID, 0);
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return RedirectToAction("getLessonsOfCourse", "Lesson", new { action = "getLessonsOfCourse", controller = "Lesson", id = lesson.ID });
            }
        
        }

        //------------------Delete Lesson-------------

        [HttpPost]
        public ActionResult DeleteLesson(int id)
        {
            lesson_DAL.DeleteLesson(id);
            ViewBag.Lessons = lesson_DAL.LessonesOfOneCourse(Convert.ToInt32(Session["CourseId"]),0);
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return RedirectToAction("getLessonsOfCourse", "Lesson", new { action = "getLessonsOfCourse", controller = "Lesson", id = Convert.ToInt32(Session["CourseId"]) });
            }
        }
    }
}
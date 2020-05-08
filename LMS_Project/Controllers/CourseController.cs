using LMS_Project.DAL;
using LMS_Project.Models;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS_Project.Controllers
{
    public class CourseController : Controller
    {
        Category_DAL dal = new Category_DAL();
        Course_DAL Course_DAL = new Course_DAL();
        List<SelectListItem> selectList = new List<SelectListItem>();
        // GET: Course
        [HttpGet]
        public ActionResult AddCourse()
        {
            
            foreach (var item in dal.AllCategory())
            {
                selectList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryId.ToString() });
            }

            ViewBag.CategoryList = selectList;
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
        public ActionResult AddCourse(Course course, HttpPostedFileBase image)
        {

            string path = Path.Combine(Server.MapPath("~/Upload/"), image.FileName);
            image.SaveAs(path);
            course.CourseImage = image.FileName;
            course.InsId = LoginViewModel.InsId;

            Course_DAL.AddCourse(course);
            foreach (var item in dal.AllCategory())
            {
                selectList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryId.ToString() });
            }

            ViewBag.CategoryList = selectList;
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }
        //------------Edit Course------------------
        [HttpGet]
        public ActionResult EditCourse(int id)
        {

            foreach (var item in dal.AllCategory())
            {
                selectList.Add(new SelectListItem() { Text = item.CategoryName, Value = item.CategoryId.ToString() });
            }

            ViewBag.CategoryList = selectList;
            Course course = Course_DAL.GetCourseById(id);

            ViewBag.Course = course.CourseTitle;
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View(course);
            }
        }
        [HttpPost]
        public ActionResult EditCourse(Course course, HttpPostedFileBase image)
        {

            string path = Path.Combine(Server.MapPath("~/Upload/"), image.FileName);
            image.SaveAs(path);
            course.CourseImage = image.FileName;
            course.InsId = LoginViewModel.InsId;

            Course_DAL.EditCourse(course);

            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return RedirectToAction("InsProfile", "InstructorProfile");
            }
            }

        //------------------Delete course-------------
        //[HttpGet]
        //public ActionResult DeleteCourse()
        //{
        //    return View();
        //}
        [HttpPost]
        public ActionResult DeleteCourse(int? id)
        {
            Course_DAL.DeleteCourse(id);
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return RedirectToAction("InsProfile", "InstructorProfile");
            }
        }
    }

}

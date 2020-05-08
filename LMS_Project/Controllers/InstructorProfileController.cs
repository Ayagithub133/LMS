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
    public class InstructorProfileController : Controller
    {
        Instructor_DAL instructor_DAL = new Instructor_DAL();
        Course_DAL course_DAL = new Course_DAL();
        // GET: InstructorProfile
        [HttpGet]
        public ActionResult InsProfile(int id=0 )
        {
            Session["Id"] = 0;
            Instructor instructor = null;
            if (Session["Instructor"] != null)
            {
                instructor = instructor_DAL.getInstructorById(id);
                Session["Id"] = instructor.InsId;
                Session["Visitor"] = instructor;
                Session.Timeout = DateTime.Now.Month;

            }
            else
            {
                instructor = instructor_DAL.getInstructorById(LoginViewModel.InsId);
                Session["Id"] = instructor.InsId;
                Session["Instructor"] = instructor;
                Session.Timeout = DateTime.Now.Month;

                LoginViewModel.FName = instructor.FName;
                LoginViewModel.LName = instructor.LName;
                LoginViewModel.Image = instructor.InsImage;
            }
            
            return View(instructor);
        }
        //Edit Profile------------------------------
        [HttpGet]
        public ActionResult EditProfile()
        { return View(instructor_DAL.getInstructorById(LoginViewModel.InsId)); }

        [HttpPost]
        public ActionResult EditProfile( Instructor instructor , HttpPostedFileBase[] upload)
        {
            if (upload != null)
            {
                string imagePath = Path.Combine(Server.MapPath("~/Upload/"), upload[0].FileName);
                upload[0].SaveAs(imagePath);
                instructor.InsImage = upload[0].FileName;

                string videoPath = Path.Combine(Server.MapPath("~/Videos/"), upload[1].FileName);
                upload[1].SaveAs(videoPath);
                instructor.IntrVideo = upload[1].FileName;



                instructor.InsId = LoginViewModel.InsId;
                instructor_DAL.EditProfile(instructor);
                LoginViewModel.LoginView(instructor);
                return RedirectToAction("InsProfile", "InstructorProfile");
            }
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }
        //------------------my course----------------
        //[HttpGet]
        //public ActionResult MyCourses(int index=0)
        //{
        //    float pages = 0.0f;


        //    pages = course_DAL.countOfCourses(LoginViewModel.InsId) / 12.0f;
        //    if (pages != 0.0f)
        //    {
        //        if ((pages - (int)pages) < 1)
        //        {
        //            ViewBag.pageNumbers = (int)pages + 1;

        //        }
        //    }
        //    else
        //    {

        //        ViewBag.pageNumbers = pages;
        //    }
        //    Instructor instructor = (Instructor)Session["Instructor"];
        //    //instructor.courses = course_DAL.CoursesOfOneInstructor(LoginViewModel.InsId, index);
        //    return View(instructor);
        //}

       
    }
}
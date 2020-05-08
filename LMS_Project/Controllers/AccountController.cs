using LMS_Project.DAL;
using LMS_Project.Models;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LMS_Project.Controllers
{
    public class AccountController : Controller
    {
        Instructor_DAL instructor_DAL = new Instructor_DAL();
        Course_DAL course_DAL = new Course_DAL();
        Account_DAL account = new Account_DAL();

        // GET: Account
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Instructor instructor , string value , HttpPostedFileBase[] upload)
        {
            if (upload != null)
            {
                if (value == "Instructor")
                {
                    string imagePath = Path.Combine(Server.MapPath("~/Upload/"), upload[0].FileName);
                    upload[0].SaveAs(imagePath);
                    instructor.InsImage = upload[0].FileName;

                    string videoPath = Path.Combine(Server.MapPath("~/Videos/"), upload[1].FileName);
                    upload[1].SaveAs(videoPath);
                    instructor.IntrVideo = upload[1].FileName;
                   

                    //////////////////////
                   
                    instructor.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(instructor.Password,"SHA1");
                   
                    instructor_DAL.NewInstructor(instructor);
                }
            }
            return RedirectToAction("SignIn");
        }

        //--------------------------------
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string Email , string Password , string value  )
        {
           bool check ;
         
            if (value == "Instructor")
            {
                check = instructor_DAL.LoginInstructor(Email, FormsAuthentication.HashPasswordForStoringInConfigFile(Password,"SHA1"));
                
                if (check)
                {
                    
                    LoginViewModel.Email = Email;
                    return RedirectToAction("InsProfile", "InstructorProfile", new { action = "InsProfile", controller = "InstructorProfile"});
                }
                else
                {
                    ViewBag.Invalid = "Invalid Email Or Password, Please Enter Correct Info ";
                    return View();
                }
            }
            return View();
        }

        ///Reset Password///
        ///
        [HttpGet]
        public ActionResult ResetPassword()
        {
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
        public ActionResult ResetPassword(string Email)
        {
            ViewBag.InValidEmaile = "";
           Guid uniqueId = account.spResetPassword(Email);
            Session["uniqueId"] = uniqueId;
            if (uniqueId != null)
            {
                Session["Text"] = "Please Chech Your Email";
                return RedirectToAction("Texts", "Account");
            }
            else
            {
                ViewBag.InValidEmaile = "Please Enter Valid Email";
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
        //////////////change password
        ///   [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string Password)
        {
            Password = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
            bool val = account.ChangePassword(Password,(Guid)Session["uniqueId"]);
            if (val)
            {
                Session["Text"] = "Password Has Changed";
                return RedirectToAction("Texts", "Account");
            }
            else { ViewBag.ChangePass = "Password Reset Link has expired or is changed"; }
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }
        ///checkYourEmail
        ///
        [HttpGet]
        public ActionResult Texts()
        {
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
        public ActionResult Search(string Type, string Name)
        {
            User user = account.SearchByName(Type, Name);
          
                Session["Lists"] = user;
            Session.Timeout = DateTime.Now.Month;
            if (user.Instructors != null)
            {
                List<Instructor> ins = user.Instructors;
            }
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return RedirectToAction("SearchResult", "Account");
            }
        }

        [HttpGet]
        public ActionResult SearchResult()
        {
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            account.Logout(LoginViewModel.InsId);
            Session["Instructor"] = null;
            Session["Visitor"] = null;
            
          return  RedirectToAction("SignIn", "Account");

        }
    }
}
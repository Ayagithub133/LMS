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
   
    public class MessageController : Controller
    {
        Instructor visitor; 
        SendMessageToDB sendMessage = new SendMessageToDB();
        // GET: Message
        [HttpGet]
        public ActionResult AddMessage()
        {
            visitor = Session["Visitor"] as Instructor;
            ViewBag.Messages = getAllMessages(LoginViewModel.InsId, visitor.InsId);
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
        public ActionResult AddMessage(string Text , HttpPostedFileBase File)
        {
            
            visitor = Session["Visitor"] as Instructor;
            UserMessage mes = new UserMessage();
            if (File == null && Text!=null)
            {
                mes.Message = Text;
                mes.Type = "TEXT";
            }
            else 
            {
                string str = Extension(File).ToLower();
                if (str == ".jpg" || str == ".png" || str == ".gif")
                {
                    string path = Path.Combine(Server.MapPath("~/Upload/"), File.FileName);
                    File.SaveAs(path);
                    mes.Message= File.FileName;
                    mes.Type = "IMAGE";
                }
                else if (str==".mp4")
                {
                    string path = Path.Combine(Server.MapPath("~/Videos/"), File.FileName);
                    File.SaveAs(path);
                    mes.Message = File.FileName;
                    mes.Type = "VIDEO";
                }
                else if (str == ".mp3")
                {
                    string path = Path.Combine(Server.MapPath("~/Audio/"), File.FileName);
                    File.SaveAs(path);
                    mes.Message = File.FileName;
                    mes.Type = "AUDIO";
                }
            }
            mes.TimeSend = DateTime.Now;
            mes.InstructorSender = LoginViewModel.InsId;
            mes.SenderFName = LoginViewModel.FName;
            mes.RecieverFName = visitor.FName;
            mes.InstructorReciever = visitor.InsId;
            sendMessage.AddMessage(mes);
           ViewBag.Messages = getAllMessages(LoginViewModel.InsId, visitor.InsId);
            if (Session["Instructor"] == null)
            {
                return RedirectToAction("SignIn", "Account");
            }
            else
            {
                return View();
            }
        }


      private List<UserMessage> getAllMessages(int InsId,int ReciverId )
        {
         List<UserMessage> messages= sendMessage.GetAllMessage(InsId, ReciverId);
            return messages;
        }
        private string Extension(HttpPostedFileBase File)
        {
            string str = null;
            int i = 0;
            while(File.FileName.Length>i)
            {
               if(File.FileName[i]!='.')
                {
                    i++;
                    continue;
                }
                else
                {
                     str = File.FileName.Substring(i);
                    return str;
                }
            }

            return str;
        }

       
    }
}
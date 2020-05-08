using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS_Project.ViewModels
{
    public class UserMessage
    {
        public int MessageId { set; get; }

        public string Message{ set; get; }

        public DateTime TimeSend { set; get; }

        public int InstructorSender { set; get; }

        public int InstructorReciever { set; get; }

        public string SenderFName { set; get; }

        public string RecieverFName { set; get; }

        public string InstructorImage { set; get; }
        public string From { set; get; }
        public Instructor instructor { set; get; }
        
        public string Type { set; get; }

    }
}
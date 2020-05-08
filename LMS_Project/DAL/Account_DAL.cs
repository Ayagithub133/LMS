using LMS_Project.Models;
using LMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace LMS_Project.DAL
{
    public class Account_DAL
    {
        ProcedureForInserting procedure = new ProcedureForInserting();
        Guid id;
        [Function(Name ="spResetPassword")]
        public Guid spResetPassword(string Email)
        {
            SqlConnection con = null;
            
            try {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("spResetPassword",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    if (Convert.ToBoolean(read["returnValue"]))
                    {
                        id = (Guid)read["uniqueId"];

                        SendEmail(id, read["FName"].ToString(), read["Email"].ToString());
                    }
                    }
                return id;
                }
            catch (Exception ex){
                
                return id; }
            finally { con.Close(); }
        }
        //---------------------------Template for stored procedure
        public bool ChangePassword(string password,Guid uniqueId)
        {
            ProcedureForInserting procedureForInserting =
                new ProcedureForInserting();


            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter()
                { ParameterName="@Password",
                 Value = password,
                },
                 new SqlParameter()
                { ParameterName="@GUID",
                 Value =uniqueId
                }
               
               
            };
            return procedureForInserting.StoredProc("spChangedPassword", parameters);
        }
       
        //-------------------------sendEmail
        private void SendEmail(Guid uniqueId ,string fName ,string Email)
        {
            MailMessage mailMessage = new MailMessage("email(from)", Email);
            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("Dear" + fName + ",<br/><br/>");
            sbEmailBody.Append("Please Click on the following link to reset your password");
            sbEmailBody.Append("<br/>");
            sbEmailBody.Append("https://localhost:/LMS_Project/Account/ChangePassword/uniqueId");//write host
            sbEmailBody.Append("<br/><br/>");
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = sbEmailBody.ToString();
            mailMessage.Subject = "Reset Your Password";
            SmtpClient smtpclient = new SmtpClient("smtp.gmail.com", 587);
            smtpclient.Credentials = new System.Net.NetworkCredential()
            { 
                UserName="Write Company Email",
                Password="Write password of email"
                
            };
            smtpclient.EnableSsl = true;
            smtpclient.Send(mailMessage);
        }
        //------------------Search By Name--------------------//
        [Function (Name = "SearchByName")]
        public User SearchByName(string type,string Name)
        {

            SqlConnection con = null;
            Instructor instructor = null;
            List<Instructor> instructors = new List<Instructor>();
            User user = new User();
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("SearchByName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Name", Name);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                if (type == "Instructor")
                {
                    while (read.Read())
                    {
                     instructor = new Instructor();

                        instructor.Email = read["Email"].ToString();
                        instructor.InsImage = read["InsImage"].ToString();
                        instructor.FName = read["FName"].ToString();
                        instructor.LName = read["LName"].ToString();
                        instructor.Password = read["Password"].ToString();
                        instructor.InsId = Convert.ToInt32(read["InsID"]);
                        instructor.IntrVideo = read["Introductory_Video"].ToString();
                        instructor.Experties = read["Expertise"].ToString();
                        instructors.Add(instructor);
                    }
                    //user.Instructors = new List<Instructor>();
                    user.Instructors = instructors;
                    
                }
                return user;

            }
            catch { return user; }
            finally { con.Close(); }
        }
        [Function(Name ="Logout")]
        public void Logout(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter{
                    ParameterName="@InsId",
                    Value=id
                }
                
            };
            procedure.StoredProc("Logout", parameters);
        }
    }
}
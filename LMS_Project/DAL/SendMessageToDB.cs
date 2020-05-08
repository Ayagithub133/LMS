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
    public class SendMessageToDB
    {
        [Function(Name ="AddMessage")]
        public void AddMessage(UserMessage message)
        {
            List<SqlParameter> parameters = new List<SqlParameter>()
            {
                new SqlParameter()
                {
                    ParameterName ="@Text",
                    Value =message.Message
                },
                new SqlParameter()
                {
                    ParameterName ="@TimeSend",
                    Value =message.TimeSend
                },
                new SqlParameter()
                {
                    ParameterName ="@InstructorSender",
                    Value =message.InstructorSender
                },
                 new SqlParameter()
                {
                    ParameterName ="@InstructorReciver",
                    Value =message.InstructorReciever
                },
                   new SqlParameter()
                {
                    ParameterName ="@RecieverFName",
                    Value =message.RecieverFName
                },
                 new SqlParameter()
                {
                    ParameterName ="@SenderFName",
                    Value =message.SenderFName
                }
                ,new SqlParameter()
                {
                    ParameterName="@From",
                    Value="From"+message.InstructorSender
                }
                ,
                 new SqlParameter()
                {
                    ParameterName ="@Type",
                    Value =message.Type
                }
            };
            ProcedureForInserting procedure = new ProcedureForInserting();
            procedure.StoredProc("AddMessage", parameters);
        }
        //----------------------------------------------------------------
        [Function(Name = "GetAllMessage")]
        public List<UserMessage> GetAllMessage(int Sender , int Reciever)
        {
            SqlConnection con = null;
            List<UserMessage> messages = new List<UserMessage>();
            UserMessage message = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("GetAllMessage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InstructorSender",Sender);
                cmd.Parameters.AddWithValue("@InstructorReciver", Reciever);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while(read.Read())
                {
                    message = new UserMessage();
                    message.Message = read["Text"].ToString();
                    message.TimeSend =Convert.ToDateTime( read["TimeSend"]);
                    message.InstructorSender =Convert.ToInt32(read["InstructorSender"]);
                    message.InstructorReciever = Convert.ToInt32(read["InstructorReciver"]);
                    message.SenderFName = read["SenderFName"].ToString();
                    message.RecieverFName = read["RecieverFName"].ToString();
                    message.From = read["From"].ToString();
                    message.Type = read["Type"].ToString();
                    messages.Add(message);
                }
                return messages;
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return messages;
            }
            finally { con.Close(); }
        }


    }
}
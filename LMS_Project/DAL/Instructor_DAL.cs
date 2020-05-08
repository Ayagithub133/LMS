using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using LMS_Project.Models;
using System.Data.SqlClient;
using System.Data;
using LMS_Project.ViewModels;
using System.Diagnostics;

namespace LMS_Project.DAL
{
   
    public class Instructor_DAL
    {
        Course_DAL CoursesList = new Course_DAL();

        [Function(Name = "NewInstructor")]
        public void NewInstructor(Instructor instructor)
        {
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("NewInstructor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FName", instructor.FName);
                cmd.Parameters.AddWithValue("@LName", instructor.LName);
                cmd.Parameters.AddWithValue("@Email", instructor.Email);
                cmd.Parameters.AddWithValue("@Password", instructor.Password);
                cmd.Parameters.AddWithValue("@InsImage", instructor.InsImage);
                cmd.Parameters.AddWithValue("@Introductory_Video", instructor.IntrVideo);
                cmd.Parameters.AddWithValue("@Expertise", instructor.Experties);
                
                con.Open();
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            { Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.Source);
                Trace.WriteLine(ex.TargetSite);
                
            }
            finally
            {
                con.Close();
            }
        }
            //*********************Login*****************************//
            [Function(Name = "LoginInstructor")]
            public bool LoginInstructor(string Email ,string Password)
            {
               SqlConnection con = null;
               bool returnvalue = false;


            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("LoginInstructor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email",Email);
                cmd.Parameters.AddWithValue("@Password",Password);
              
                //
                SqlParameter sqlParameter2 = new SqlParameter();
                sqlParameter2.SqlDbType = SqlDbType.Int;
                sqlParameter2.ParameterName = "@InsId";
                sqlParameter2.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParameter2);
                //
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.SqlDbType = SqlDbType.Bit;
                sqlParameter.ParameterName = "@check";
                sqlParameter.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(sqlParameter);
                con.Open();
                cmd.ExecuteNonQuery();
                returnvalue = Convert.ToBoolean( sqlParameter.Value);
                LoginViewModel.InsId = Convert.ToInt32(sqlParameter2.Value);
                con.Close();
                return returnvalue;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return returnvalue;
            }
            finally
            { con.Close(); }
            }
        //Edit Profile-------------------------
        [Function(Name = "EditProfile")]
        public void EditProfile(Instructor instructor)
        {
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("EditProfile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsId", instructor.InsId);
                cmd.Parameters.AddWithValue("@FName", instructor.FName);
                cmd.Parameters.AddWithValue("@LName", instructor.LName);
                cmd.Parameters.AddWithValue("@Email", instructor.Email);
                cmd.Parameters.AddWithValue("@Password", instructor.Password);
                cmd.Parameters.AddWithValue("@InsImage", instructor.InsImage);
                cmd.Parameters.AddWithValue("@Url", instructor.Url);
                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch { }
            finally
            {
                con.Close();
            }
        }
            //------------------------------------------------
        [Function(Name = "getInstructorById")]
        public Instructor getInstructorById(int id)
        { Instructor instructor = null;
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("getInstructorById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    instructor = new Instructor();
                    instructor.Online = Convert.ToBoolean(read["Online"]);
                    instructor.Email = read["Email"].ToString();
                    instructor.InsImage = read["InsImage"].ToString();
                    instructor.FName = read["FName"].ToString();
                    instructor.LName = read["LName"].ToString();
                    instructor.Password = read["Password"].ToString();
                    instructor.InsId = Convert.ToInt32(read["InsID"]);
                    instructor.IntrVideo = read["Introductory_Video"].ToString();
                    instructor.Experties = read["Expertise"].ToString();
                    
                    break;
                }
                
                read.Close();
                con.Close();
                instructor.courses = CoursesList.CoursesOfOneInstructor2(id);

                return instructor;  
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex.Message);
                return instructor;
            }
            finally { con.Close() ; }
            }
        }
    } 
    
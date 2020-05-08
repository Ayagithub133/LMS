using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace LMS_Project.DAL
{
    public class Course_DAL
    {
        [Function(Name = "AddCourse")]
        public void AddCourse(Course course)
        {
            SqlConnection con = null;
            
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("AddCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseTitle",course.CourseTitle);
                cmd.Parameters.AddWithValue("@CourseDescription",course.CourseDescription);
                cmd.Parameters.AddWithValue("@CourseImage",course.CourseImage);
                cmd.Parameters.AddWithValue("@InsId",course.InsId);
                cmd.Parameters.AddWithValue("@CategoryId",course.CategoryId);
                cmd.Parameters.AddWithValue("@Duration",course.Duration);
                cmd.Parameters.AddWithValue("@StartCourse",course.StartCourse);
                cmd.Parameters.AddWithValue("@EndCourse",course.EndCourse);
                cmd.Parameters.AddWithValue("@Price",course.Price);
                cmd.Parameters.AddWithValue("@Level", course.Level);
                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch { }
            finally { con.Close(); }
        }

        //----------------------Edit course----------------
        [Function(Name = "EditCourse")]
        public void EditCourse(Course course)
        {
            SqlConnection con = null;

            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("EditCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", course.Id);
                cmd.Parameters.AddWithValue("@CourseTitle", course.CourseTitle);
                cmd.Parameters.AddWithValue("@CourseDescription", course.CourseDescription);
                cmd.Parameters.AddWithValue("@CourseImage", course.CourseImage);
                
                
                cmd.Parameters.AddWithValue("@Duration", course.Duration);
                cmd.Parameters.AddWithValue("@StartCourse", course.StartCourse);
                cmd.Parameters.AddWithValue("@EndCourse", course.EndCourse);
                cmd.Parameters.AddWithValue("@Price", course.Price);
                cmd.Parameters.AddWithValue("@Level", course.Level);
                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch { }
            finally { con.Close(); }
        }
        //------------------Delete Course----------------
        [Function(Name = "DeleteCourse")]
        public void DeleteCourse(int? id)
        {
            SqlConnection con = null;

            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("DeleteCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch {  }
            finally { con.Close(); }

            }
        //--------------------------ALL Courses--------------------------
        [Function(Name = "AllCourses")]
        public List<Course> AllCourses()
        {
           Course course = null;
            List<Course> courses = new List<Course>();
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("AllCourses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    course = new Course();
                    course.Id = Convert.ToInt32(read["ID"]);
                    course.CourseTitle = read["CourseTitle"].ToString();
                    
                    courses.Add(course);
                }
                read.Close();
                con.Close();
                return courses;

            }
            catch { return courses; }
            finally { con.Close(); }
        }
        //----------------courses of one instructor ------------
        [Function(Name = "CoursesOfOneInstructor2")]
        public  List<Course> CoursesOfOneInstructor2( int id /*, int index*/)
        {
            Course course = null;
            List<Course> courses = new List<Course>();
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("CoursesOfOneInstructor2",con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsId", id);

                //cmd.Parameters.AddWithValue("@index", index);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    course = new Course();
                    course.Id = Convert.ToInt32(read["ID"]);
                    course.CourseTitle = read["CourseTitle"].ToString();
                    course.CourseDescription = read["CourseDescription"].ToString();
                    course.CourseImage = read["CourseImage"].ToString();
                    course.CategoryId =Convert.ToInt32(read["CategoryId"]);
                    course.Duration = Convert.ToDecimal(read["Duration"]);
                    course.StartCourse = Convert.ToDateTime(read["StartCourse"]);
                    course.EndCourse = Convert.ToDateTime(read["EndCourse"]);
                    course.Price = Convert.ToDecimal(read["Price"]);
                    course.Level=(Level) read["Level"];
                    courses.Add(course);
                }
               
                return courses;

            }
            catch (Exception ex){
              
             
                return courses; }
                finally { con.Close(); }
           
        }
        //----------------------countOfCourses 
        [Function(Name = "countOfCourses ")]
        public float countOfCourses(int id)
        {
            SqlConnection con = null;
            float count = 0.0f;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("countOfCourses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@InsId", id);
                SqlParameter result = new SqlParameter();
                result.SqlDbType = SqlDbType.Int;
                result.ParameterName = "@count1";
                result.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(result);
                con.Open();
                cmd.ExecuteNonQuery();
                count = Convert.ToInt64(result.Value);
                return count;
            }
            catch { return count; }
            finally{ con.Close(); }
        }
        //------------GetCourseById-----------
        [Function(Name = "GetCourseById")]
        public Course GetCourseById(int id)
        {
            Course course = null;
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("GetCourseById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@courseId", id);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while(read.Read())
                {
                    course = new Course();
                    course.Id = Convert.ToInt32(read["ID"]);
                    course.CourseTitle = read["CourseTitle"].ToString();
                    course.CourseDescription = read["CourseDescription"].ToString();
                    course.CourseImage = read["CourseImage"].ToString();
                    course.CategoryId = Convert.ToInt32(read["CategoryId"]);
                    course.Duration = Convert.ToDecimal(read["Duration"]);
                    course.StartCourse = Convert.ToDateTime(read["StartCourse"]);
                    course.EndCourse = Convert.ToDateTime(read["EndCourse"]);
                    course.Price = Convert.ToDecimal(read["Price"]);
                    course.Level = (Level)read["Level"];
                    break;
                }
                return course;
            }
            catch { return course; }
            finally { con.Close(); }
        }
    }
}
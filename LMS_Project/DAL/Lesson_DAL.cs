using LMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS_Project.DAL
{
    public class Lesson_DAL
    {
        //--------------------------AddLesson------------
        [Function(Name = "AddLesson")]
        public void AddLesson(Lesson lesson)
        {
            SqlConnection con = null;

            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("AddLesson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LessonTitle", lesson.LessonTitle);
                cmd.Parameters.AddWithValue("@TextContent", lesson.TextContent);
                cmd.Parameters.AddWithValue("@TextContentSize", lesson.TextContentSize);
                cmd.Parameters.AddWithValue("@video", lesson.video);
                cmd.Parameters.AddWithValue("@VideoSize", lesson.VideoSize);
                cmd.Parameters.AddWithValue("@ID", lesson.ID);
                cmd.Parameters.AddWithValue("@LessonImage", lesson.LessonImage);

                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch { }
            finally { con.Close(); }
        }
        ///---------------------Delete Lesson------------
        ///
        [Function(Name = "DeleteLesson")]
        public void DeleteLesson(int id)
        {
            SqlConnection con = null;

            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("DeleteLesson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch { }
            finally { con.Close(); }

        }
        //----------------Edit Lesson------------
        [Function(Name = "EditLesson")]
        public void EditLesson(Lesson lesson)
        {
            SqlConnection con = null;

            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("EditLesson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LessonId", lesson.LessonId);
                
                cmd.Parameters.AddWithValue("@LessonTitle", lesson.LessonTitle);
                cmd.Parameters.AddWithValue("@video", lesson.video);
                cmd.Parameters.AddWithValue("@VideoSize", lesson.VideoSize);
                cmd.Parameters.AddWithValue("@TextContent", lesson.TextContent);
                cmd.Parameters.AddWithValue("@TextContentSize", lesson.TextContentSize);
                cmd.Parameters.AddWithValue("@ID", lesson.ID);
                cmd.Parameters.AddWithValue("@LessonImage", lesson.LessonImage);

                con.Open();
                cmd.ExecuteNonQuery();

            }
            catch { }
            finally { con.Close(); }
        }
        //----------------------Lesson by id-----------
        [Function(Name = "GetLessonById")]
        public Lesson GetLessonById(int lessonid)
        {
            Lesson lesson = null;
           
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("GetLessonById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LessonId", lessonid);

               
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    lesson = new Lesson();
                    lesson.LessonId = Convert.ToInt32(read["LessonId"]);
                    lesson.LessonImage = read["LessonImage"].ToString();
                    lesson.LessonTitle = read["LessonTitle"].ToString();
                    lesson.video = read["video"].ToString();
                    lesson.VideoSize = Convert.ToInt32(read["VideoSize"]);
                    lesson.TextContent = read["TextContent"].ToString();
                    lesson.TextContentSize = Convert.ToInt32(read["TextContentSize"]);

                    break;
                }

                return lesson;

            }
            catch (Exception ex)
            {


                return lesson;
            }
            finally { con.Close(); }

        }
        //-------------------------retrive lessons-----------------
        //----------------courses of one instructor ------------
        [Function(Name = "LessonesOfOneCourse")]
        public List<Lesson> LessonesOfOneCourse(int courseid, int index)
        {
            Lesson lesson = null;
            List<Lesson> lessones = new List<Lesson>();
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("LessonesOfOneCourse", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CourseId", courseid);

                cmd.Parameters.AddWithValue("@index", index);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    lesson= new Lesson();
                    lesson.LessonId = Convert.ToInt32(read["LessonId"]);
                    lesson.LessonImage = read["LessonImage"].ToString();
                    lesson.LessonTitle = read["LessonTitle"].ToString();
                    lesson.video = read["video"].ToString();
                    lesson.VideoSize = Convert.ToInt32(read["VideoSize"]);
                    lesson.TextContent = read["TextContent"].ToString();
                    lesson.TextContentSize = Convert.ToInt32(read["TextContentSize"]);
                   
                    lessones.Add(lesson);
                }

                return lessones;

            }
            catch (Exception ex)
            {


                return lessones;
            }
            finally { con.Close(); }

        }
        //----------------------countOfLessons 
        [Function(Name = "countOfLessons")]
        public float countOfLessons(int? courseid)
        {
            SqlConnection con = null;
            float count = 0.0f;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("countOfLessons ", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@courseId", courseid);
                SqlParameter result = new SqlParameter();
                result.SqlDbType = SqlDbType.Int;
                result.ParameterName = "@count";
                result.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(result);
                con.Open();
                cmd.ExecuteNonQuery();
                count = Convert.ToInt64(result.Value);
                return count;
            }
            catch { return count; }
            finally { con.Close(); }
        }

    }
}
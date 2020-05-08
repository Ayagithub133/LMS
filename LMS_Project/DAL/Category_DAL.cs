using LMS_Project.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq.Mapping;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS_Project.DAL
{
    public class Category_DAL
    {
        [Function(Name="AllCategory")]
        public List<Category> AllCategory()
        {
            Category category = null;
            List<Category> categories = new List<Category>();
            SqlConnection con = null;
            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand("AllCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                Hashtable hash = new Hashtable();
               
                
                while(read.Read())
                {
                    
                    category = new Category();
                    category.CategoryId = Convert.ToInt32(read["CategoryId"]);
                    category.CategoryName = read["CategoryName"].ToString();
                    category.CategoryImage = read["CategoryImage"].ToString();
                    category.Description = read["Description"].ToString();
                    categories.Add(category);
                }
                read.Close();
                con.Close();
                return categories;

            }
            catch{ return categories; }
            finally { con.Close(); }
        }
    }
}
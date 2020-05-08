using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMS_Project.DAL
{
    public class Connection
    {
        public  static SqlConnection connectToDb()
        {
            return new SqlConnection(@"Data Source=PC\SQLEXPRESS;Initial Catalog=LMS_Database;Integrated Security=True");
        }
    }
}
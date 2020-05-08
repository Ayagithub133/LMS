using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace LMS_Project.DAL
{
    public class ProcedureForInserting
    {
        public bool StoredProc(string storedProcName, List<SqlParameter> prams)
        {
            SqlConnection con = null;

            try
            {
                con = Connection.connectToDb();
                SqlCommand cmd = new SqlCommand(storedProcName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter para in prams)
                {
                    cmd.Parameters.Add(para);
                }
                con.Open();
                return (Convert.ToBoolean(cmd.ExecuteScalar()));
            }
            catch(Exception ex) {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.Source);
                return false; }
            finally { con.Close(); }
        }
    }
}
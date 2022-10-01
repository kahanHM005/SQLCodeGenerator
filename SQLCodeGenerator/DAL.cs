using System.Data;
using System.Data.SqlClient;

namespace SQLCodeGenerator
{
    public class DAL
    {
        public static string? ConnectionString { get; set; }

        public static int Execute(String sQuery, CommandType type, params SqlParameter[] Params)
        {
            using (SqlConnection con = new SqlConnection(DAL.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sQuery, con))
                {
                    cmd.CommandType = type;
                    foreach (SqlParameter prm in Params)
                        cmd.Parameters.Add(prm);
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static SqlDataReader GetDataReader(String sQuery, params SqlParameter[] Params)
        {
            using (SqlConnection con = new SqlConnection(DAL.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sQuery, con))
                {
                    cmd.CommandType = CommandType.Text;
                    foreach (SqlParameter prm in Params)
                        cmd.Parameters.Add(prm);
                    return cmd.ExecuteReader();
                }
            }
        }

        public static SqlDataReader GetDataReader(String sQuery, CommandType ct, params SqlParameter[] Params)
        {
            using (SqlConnection con = new SqlConnection(DAL.ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(sQuery, con))
                {
                    cmd.CommandType = ct;
                    foreach (SqlParameter prm in Params)
                        cmd.Parameters.Add(prm);
                    return cmd.ExecuteReader();
                }
            }
        }

        public static DataSet GetDataSet(String sQuery, params SqlParameter[] Params)
        {
            using (SqlConnection con = new SqlConnection(DAL.ConnectionString))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sQuery, con))
                {
                    foreach (SqlParameter prm in Params)
                        da.SelectCommand.Parameters.Add(prm);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
        }

        public static SqlParameter CreateParameter(string Name, SqlDbType DBType, int Width, object prmValue)
        {
            SqlParameter NewPrm = new SqlParameter(Name, DBType, Width);
            NewPrm.Value = prmValue;
            return NewPrm;
        }

        public static DataSet GetDataSet(String sQuery, CommandType type, params SqlParameter[] Params)
        {
            using (SqlConnection con = new SqlConnection(DAL.ConnectionString))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(sQuery, con))
                {
                    da.SelectCommand.CommandType = type;
                    foreach (SqlParameter prm in Params)
                        da.SelectCommand.Parameters.Add(prm);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
        }
        public static String GetNewPassword(bool UpperCase = true, bool LowerCase = true, bool digits = true, String SpecialChar = "#@")
        {
            var chars = "";

            if (UpperCase)
                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (LowerCase)
                chars += "abcdefghijklmnopqrstuvwxyz";

            if (digits)
                chars += "0123456789";

            chars += SpecialChar;

            var random = new Random();
            String newPassword = new string(
            Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
            return newPassword;
        }
    }
}



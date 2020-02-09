using iii.CUser;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace viewmodel.Models
{
    public class CUserFactory
    {
        public CUser getByAccount(string txtAccount)
        {

            List<CUser> mylist = selectbysql($"select * from tUser where fAccount='{txtAccount}'");
            if (mylist.Count == 0)
                return null;
            return mylist[0];
        }

        public List<CUser> getall()
        {

            return selectbysql("select * from tUser");
        }

        private List<CUser> selectbysql(string sql)
        {
            
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=.;Initial Catalog=dbMyProject;Integrated Security=True";
            SqlDataAdapter myad = new SqlDataAdapter(sql, conn);
            conn.Open();
            DataSet myds = new DataSet();
            myad.Fill(myds);
            conn.Close();
            List<CUser> mylist = new List<CUser>(); ;
            foreach (DataRow item in myds.Tables[0].Rows)
            {
                CUser myuser = new CUser();
                myuser.fAccount = item["fAccount"].ToString();
                myuser.fPassword = item["fPassword"].ToString();
                myuser.fName = item["fName"].ToString();
                myuser.fDepartment = item["fDepartment"].ToString();
                myuser.fAuthorization = item["fAuthorization"].ToString();
                myuser.fStatus = item["fStatus"].ToString();
                myuser.fArrivingDate = item["fArrivingDate"].ToString();
                myuser.fLeavingDate = item["fLeavingDate"].ToString();
                myuser.fEmail = item["fEmail"].ToString();
                myuser.fPhone = item["fPhone"].ToString();
                myuser.fAddress = item["fAddress"].ToString();

                mylist.Add(myuser);
            }
           
            return mylist;
        }

        public void insertdb(CUser newuser)
        {
            //密碼加密寫進資料庫  
            //檢查資料庫有無同樣帳號
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=.;Initial Catalog=dbMyProject;Integrated Security=True";
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
          
            comm.Parameters.Add("fAccount", SqlDbType.NVarChar, 20).Value = newuser.fAccount.ToString();
            comm.Parameters.Add("fPassword", SqlDbType.NVarChar, 40).Value = newuser.fPassword.ToString();
            string sql = "insert into tUser (" ;
            sql += "fAccount,";
            sql += "fPassword,";
            sql += "fName,";
            sql += "fDepartment,";
            sql += "fAuthorization,";
            sql += "fStatus,";
            sql += "fArrivingDate,";
            sql += "fLeavingDate,";
            sql += "fEmail,";
            sql += "fPhone,";
            sql += "fAddress";
            sql += ") values (";
            sql += "@fAccount,";
            sql += "@fPassword,";
            sql += $"'{newuser.fName}',";
            sql += $"'{newuser.fDepartment}',";
            sql += $"'{newuser.fAuthorization}',";
            sql += $"'{newuser.fStatus}',";
            sql += $"'{newuser.fArrivingDate}',";
            sql += $"'{newuser.fLeavingDate}',";
            sql += $"'{newuser.fEmail}',";
            sql += $"'{newuser.fPhone}',";
            sql += $"'{newuser.fAddress}')";


            comm.CommandText = sql;
            conn.Open();
            comm.ExecuteNonQuery();
            conn.Close();
        }
    }
}
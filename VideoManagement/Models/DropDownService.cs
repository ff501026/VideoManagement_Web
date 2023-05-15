using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VideoManagement.Models
{
    public class DropDownService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 取得書籍類別的部分資料
        /// </summary>
        /// <returns>書籍類別下拉選單</returns>
        public List<DropDownList> GetVideoClassId()
        {
            DataTable dt = new DataTable(); //宣告一個資料表
            string sql = @"SELECT Video_CLASS_ID  As CodeId,
                                  Video_CLASS_NAME  As CodeName
                           FROM Video_CLASS(NOLOCK)"; //下sql指令
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString())) //連接db
            {
                conn.Open(); //開啟連線
                SqlCommand cmd = new SqlCommand(sql, conn); //連接db下sql指令
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //宣告一個SqlDataAdapter保存SqlCommand
                sqlAdapter.Fill(dt); //填入資料
                conn.Close(); //關閉連線
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得書籍狀態的部分資料
        /// </summary>
        /// <returns>書籍狀態下拉選單</returns>
        public List<DropDownList> GetVideoStatus(string type)
        {
            DataTable dt = new DataTable(); //宣告一個資料表
            string sql = @"SELECT CODE_ID AS CodeId,
	                              CODE_NAME AS CodeName 
	                       FROM Video_CODE(NOLOCK)
	                       WHERE CODE_TYPE = @Type"; //下sql指令
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString())) //連接db
            {
                conn.Open(); //開啟連線
                SqlCommand cmd = new SqlCommand(sql, conn); //連接db下sql指令
                cmd.Parameters.Add(new SqlParameter("@Type", type)); //將sql指令帶入參數
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //宣告一個SqlDataAdapter保存SqlCommand
                sqlAdapter.Fill(dt); //填入資料
                conn.Close(); //關閉連線
            }
            return this.MapCodeData(dt);
        }


        /// <summary>
        /// 取得借閱人的部分資料
        /// </summary>
        /// <returns>借閱人下拉選單</returns>
        public List<DropDownList> GetMemberMId()
        {
            DataTable dt = new DataTable(); //宣告一個資料表
            string sql = @"SELECT USER_ID AS CodeId,
                                  (USER_ENAME+'-'+USER_CNAME) AS CodeName
                           FROM MEMBER_M(NOLOCK)"; //下sql指令
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString())) //連接db
            {
                conn.Open(); //開啟連線
                SqlCommand cmd = new SqlCommand(sql, conn); //連接db下sql指令
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //宣告一個SqlDataAdapter保存SqlCommand
                sqlAdapter.Fill(dt); //填入資料
                conn.Close(); //關閉連線
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// Maping代碼資料
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>下拉選單</returns>
        private List<DropDownList> MapCodeData(DataTable dt)
        {
            List<DropDownList> result = new List<DropDownList>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new DropDownList()
                {
                    text = row["CodeName"]?.ToString(),
                    value = row["CodeId"]?.ToString()
                });
            }
            return result;
        }


    }
}
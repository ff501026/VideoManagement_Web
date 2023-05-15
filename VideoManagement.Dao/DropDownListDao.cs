using System;
using System.Collections.Generic;
using VideoManagement.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace VideoManagement.Dao
{
    public class DropDownListDao : IDropDownListDao
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString() //取得連接字串
        {
            return Common.ConfigTool.GetDBConnectionString("DBConn");
        }

        /// <summary>
        /// 取得影片類別的部分資料
        /// </summary>
        /// <returns>影片類別下拉選單</returns>
        public List<DropDownList> GetVideoClassId()
        {
            DataTable dt = new DataTable(); //宣告一個資料表
            string sql = @"SELECT VIDEO_CLASS_ID  As CodeId,
                                  VIDEO_CLASS_NAME  As CodeName
                           FROM VIDEO_CLASS(NOLOCK)"; //下sql指令
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString())) //連接db
            {
                conn.Open(); //開啟連線
                SqlCommand cmd = new SqlCommand(sql, conn); //連接db下sql指令
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //宣告一個SqlDataAdapter保存SqlCommand
                sqlAdapter.Fill(dt); //填入資料
                conn.Close(); //關閉連線
            }
            return MapCodeData(dt);
        }

        /// <summary>
        /// 取得影片狀態的部分資料
        /// </summary>
        /// <returns>影片狀態下拉選單</returns>
        public List<DropDownList> GetVideoStatus(string type)
        {
            DataTable dt = new DataTable(); //宣告一個資料表
            string sql = @"SELECT CODE_ID AS CodeId,
	                              CODE_NAME AS CodeName 
	                       FROM VIDEO_CODE(NOLOCK)
	                       WHERE CODE_TYPE = @Type"; //下sql指令
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString())) //連接db
            {
                conn.Open(); //開啟連線
                SqlCommand cmd = new SqlCommand(sql, conn); //連接db下sql指令
                cmd.Parameters.Add(new SqlParameter("@Type", type)); //將sql指令帶入參數
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //宣告一個SqlDataAdapter保存SqlCommand
                sqlAdapter.Fill(dt); //填入資料
                conn.Close(); //關閉連線
            }
            return MapCodeData(dt);
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
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString())) //連接db
            {
                conn.Open(); //開啟連線
                SqlCommand cmd = new SqlCommand(sql, conn); //連接db下sql指令
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //宣告一個SqlDataAdapter保存SqlCommand
                sqlAdapter.Fill(dt); //填入資料
                conn.Close(); //關閉連線
            }
            return MapCodeData(dt);
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

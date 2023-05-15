using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;


namespace VideoManagement.Models
{
    public class VideoDataService
    {
        /// <summary>
        /// 取得DB連線字串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnectionString() //取得連接字串
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 依照書本ID取得書本名稱
        /// </summary>
        /// <param name="VideoId">書本ID</param>
        /// <returns>書本名稱</returns>
        public string GetVideoNameByVideoId(int VideoId)
        {
            string VideoName = "";
            string sql = @"SELECT Video_NAME AS VideoName
                           FROM Video_DATA (NOLOCK)
                           WHERE Video_DATA.Video_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoId));
                VideoName = cmd.ExecuteScalar().ToString(); //取得第一列的第一行
                conn.Close();
            }

            return VideoName;
        }

        /// <summary>
        /// 依照書籍類別ID判斷是否存在此書籍類別
        /// </summary>
        /// <param name="VideoClassId">書籍類別ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoClassId(string VideoClassId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM Video_CLASS (NOLOCK) WHERE Video_CLASS_ID = @VideoClassId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", VideoClassId ?? string.Empty));
                result = cmd.ExecuteScalar() != null; //取得第一列的第一行
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 依照書籍ID判斷是否存在此書籍
        /// </summary>
        /// <param name="VideoId">書本ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoId(int VideoId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM Video_DATA WHERE Video_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoId));
                result = cmd.ExecuteScalar() != null; //取得第一列的第一行
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 依照書籍狀態ID判斷是否存在此狀態
        /// </summary>
        /// <param name="VideoStatusId">書本狀態ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoStatusId(string VideoStatusId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM Video_CODE (NOLOCK) 
                           WHERE CODE_ID = @VideoStatusId AND CODE_TYPE = 'Video_STATUS'";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", VideoStatusId ?? string.Empty));
                result = cmd.ExecuteScalar() != null; //取得第一列的第一行
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 依照人員ID判斷是否存在此人員
        /// </summary>
        /// <param name="userId">借閱人ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistMemberId(string userId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM MEMBER_M (NOLOCK) 
                           WHERE USER_ID = @UserId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId ?? string.Empty));
                result = cmd.ExecuteScalar() != null; //取得第一列的第一行
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 取得單一書籍資料
        /// </summary>
        /// <param name="VideoId">書本ID</param>
        /// <returns>單一書籍資料</returns>
        public Models.VideoData GetSingleVideoDataByVideoId(int VideoId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.Video_ID AS VideoId,
                                  bd.Video_CLASS_ID AS VideoClassId,
	                              bcs.Video_CLASS_NAME AS VideoClassName,
	                              bd.Video_NAME AS VideoName,
	                              bd.Video_BOUGHT_DATE AS VideoBoughtDate,
	                              bd.Video_STATUS AS VideoStatusId,
	                              bcd.CODE_NAME AS VideoStatusName,
	                              bd.Video_KEEPER AS VideoKeeperId,
	                              mm.USER_ENAME AS VideoKeeperName,
                                  bd.Video_AUTHOR AS VideoAuthor,
                                  bd.Video_PUBLISHER AS VideoPublisher,
                                  bd.Video_NOTE AS VideoNote
                        FROM Video_DATA AS bd
                        INNER JOIN Video_CLASS AS bcs 
	                        ON bcs.Video_CLASS_ID = bd.Video_CLASS_ID
                        LEFT JOIN MEMBER_M AS mm 
	                        ON mm.USER_ID = bd.Video_KEEPER
                        INNER JOIN Video_CODE AS bcd 
	                        ON bcd.CODE_ID = bd.Video_STATUS AND CODE_TYPE = 'Video_STATUS'
                        WHERE bd.Video_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //從Command取得資料存入dataAdapter
                sqlAdapter.Fill(dt); //將dataAdapter資料存入DataTable
                conn.Close();
            }

            return this.MapVideoDataToList(dt);
        }

        /// <summary>
        /// 依照條件取得書籍資料
        /// </summary>
        /// <param name="arg">查詢參數</param>
        /// <returns>多筆書籍資料</returns>
        public List<Models.VideoData> GetVideoDataByCondtion(Models.VideoDataSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.Video_ID AS VideoId,
                                  bd.Video_CLASS_ID AS VideoClassId,
	                              bcs.Video_CLASS_NAME AS VideoClassName,
	                              bd.Video_NAME AS VideoName,
	                              bd.Video_BOUGHT_DATE AS VideoBoughtDate,
	                              bd.Video_STATUS AS VideoStatusId,
	                              bcd.CODE_NAME AS VideoStatusName,
	                              bd.Video_KEEPER AS VideoKeeperId,
	                              mm.USER_ENAME AS VideoKeeperName
                          FROM Video_DATA AS bd (NOLOCK) 
                          INNER JOIN Video_CLASS AS bcs (NOLOCK)  
	                          ON bcs.Video_CLASS_ID = bd.Video_CLASS_ID
                          LEFT JOIN MEMBER_M AS mm (NOLOCK)  
	                          ON mm.USER_ID = bd.Video_KEEPER
                          INNER JOIN Video_CODE AS bcd (NOLOCK)  
	                          ON bcd.CODE_ID = bd.Video_STATUS  AND CODE_TYPE = 'Video_STATUS'
                          WHERE (bd.Video_NAME LIKE '%' + @VideoName + '%' OR @VideoName = '') AND
	                            (bd.Video_CLASS_ID = @VideoClassId OR @VideoClassId = '')AND
	                            (bd.Video_KEEPER = @VideoKeeperId OR @VideoKeeperId = '')AND
	                            (bd.Video_STATUS = @VideoStatusId OR @VideoStatusId = '')
                          ORDER BY VideoBoughtDate DESC";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoName", arg.VideoName ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", arg.VideoClassId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoKeeperId", arg.VideoKeeperId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", arg.VideoStatusId ?? string.Empty));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //從Command取得資料存入dataAdapter
                sqlAdapter.Fill(dt); //將dataAdapter資料存入DataTable
                conn.Close();
            }

            return this.MapVideoDataSearchToList(dt);
        }

        /// <summary>
        /// 依照書本ID取得借閱資料
        /// </summary>
        /// <param name="VideoId">書本ID</param>
        /// <returns>多筆借閱資料</returns>
        public List<Models.VideoLendRecord> GetVideoLendDataByVideoId(int VideoId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT BLR.LEND_DATE AS LendDate, BLR.KEEPER_ID AS KeeperId, 
	                              MM.USER_ENAME AS UserEname, MM.USER_CNAME AS UserCname
                           FROM Video_LEND_RECORD AS BLR (NOLOCK) 
                           INNER JOIN MEMBER_M AS MM (NOLOCK) 
                               ON MM.USER_ID = BLR.KEEPER_ID
                           WHERE BLR.Video_ID = @VideoId
                           ORDER BY LendDate DESC";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);//從Command取得資料存入dataAdapter
                sqlAdapter.Fill(dt);//將dataAdapter資料存入DataTable
                conn.Close();
            }

            return this.MapVideoLendRecordToList(dt);
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="VideoData">書本資料</param>
        /// <returns>新增狀態訊息</returns>
        public ResponseStatus InsertVideoData(Models.VideoData VideoData)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @" INSERT INTO Video_DATA
                            (
	                            Video_NAME, Video_CLASS_ID, Video_AUTHOR,
	                            Video_BOUGHT_DATE, Video_PUBLISHER, Video_NOTE,
	                            Video_STATUS, CREATE_DATE, CREATE_USER
                            )
                            VALUES
                            (
	                            @VideoName, @VideoClassId, @VideoAuthor, 
	                            @VideoBoughtDate, @VideoPublisher, @VideoNote,
	                            'A', GETDATE(), @CreateUser
                            )";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoName", VideoData.VideoName));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", VideoData.VideoClassId));
                cmd.Parameters.Add(new SqlParameter("@VideoAuthor", VideoData.VideoAuthor));
                cmd.Parameters.Add(new SqlParameter("@VideoBoughtDate", VideoData.VideoBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@VideoPublisher", VideoData.VideoPublisher));
                cmd.Parameters.Add(new SqlParameter("@VideoNote", VideoData.VideoNote));
                cmd.Parameters.Add(new SqlParameter("@CreateUser", Environment.MachineName));
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    responseStatus.StatusCode = (int)HttpStatusCode.OK; //要改
                    responseStatus.StatusMessage = "新增成功！";
                }
                catch (Exception)
                {
                    responseStatus.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseStatus.StatusMessage = "新增失敗！";
                }
                finally
                {
                    conn.Close();
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// 刪除書籍
        /// </summary>
        /// <param name="VideoId">書本ID</param>
        /// <returns>刪除狀態訊息</returns>
        public string DeleteVideoData(int VideoId)
        {
            string result;
            string sql = @"DELETE FROM Video_DATA 
                           WHERE (Video_DATA.Video_ID = @VideoId) AND
                                 (Video_DATA.Video_STATUS != 'B') AND
                                 (Video_DATA.Video_STATUS != 'C')";

            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoId));
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    result = "刪除成功！";
                }
                catch (Exception)
                {
                    result = "刪除失敗！";
                }
                finally
                {
                    conn.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 新增書籍
        /// </summary>
        /// <param name="VideoData">書本資料</param>
        /// <returns>修改狀態訊息</returns>
        public ResponseStatus UpdateVideoData(Models.VideoData VideoData)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @" UPDATE Video_DATA 
		                    SET Video_NAME = @VideoName, Video_CLASS_ID = @VideoClassId,
		                    Video_AUTHOR = @VideoAuthor, Video_BOUGHT_DATE = @VideoBoughtDate,
		                    Video_PUBLISHER = @VideoPublisher, Video_NOTE = @VideoNote,
		                    Video_KEEPER = @VideoKeeperId, Video_STATUS = @VideoStatusId, 
		                    MODIFY_DATE = GETDATE(), MODIFY_USER = @ModifyUser
		                    WHERE Video_DATA.Video_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoData.VideoId));
                cmd.Parameters.Add(new SqlParameter("@VideoName", VideoData.VideoName));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", VideoData.VideoClassId));
                cmd.Parameters.Add(new SqlParameter("@VideoAuthor", VideoData.VideoAuthor));
                cmd.Parameters.Add(new SqlParameter("@VideoBoughtDate", VideoData.VideoBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@VideoPublisher", VideoData.VideoPublisher));
                cmd.Parameters.Add(new SqlParameter("@VideoNote", VideoData.VideoNote));
                cmd.Parameters.Add(new SqlParameter("@VideoKeeperId", VideoData.VideoKeeperId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", VideoData.VideoStatusId));
                cmd.Parameters.Add(new SqlParameter("@ModifyUser", Environment.MachineName));
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    responseStatus.StatusCode = (int)HttpStatusCode.OK;
                    responseStatus.StatusMessage = "修改成功！";
                }
                catch (Exception)
                {
                    responseStatus.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseStatus.StatusMessage = "修改失敗！";
                }
                finally
                {
                    conn.Close();
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// 修改書籍與新增借閱紀錄
        /// </summary>
        /// <param name="VideoData">書本資料</param>
        /// <returns>修改狀態訊息</returns>
        public ResponseStatus UpdateVideoDataAndLendRecord(Models.VideoData VideoData)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @" INSERT INTO Video_LEND_RECORD
			                (
				                Video_ID, KEEPER_ID, LEND_DATE, CRE_DATE, CRE_USR
			                )
			                VALUES
			                (
				                @VideoId, @VideoKeeperId, GETDATE(), GETDATE(), @CreUsr
			                );
		                    UPDATE Video_DATA 
		                    SET Video_NAME = @VideoName, Video_CLASS_ID = @VideoClassId,
		                    Video_AUTHOR = @VideoAuthor, Video_BOUGHT_DATE = @VideoBoughtDate,
		                    Video_PUBLISHER = @VideoPublisher, Video_NOTE = @VideoNote,
		                    Video_KEEPER = @VideoKeeperId, Video_STATUS = @VideoStatusId, 
		                    MODIFY_DATE = GETDATE(), MODIFY_USER = @ModifyUser
		                    WHERE Video_DATA.Video_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", VideoData.VideoId));
                cmd.Parameters.Add(new SqlParameter("@VideoName", VideoData.VideoName));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", VideoData.VideoClassId));
                cmd.Parameters.Add(new SqlParameter("@VideoAuthor", VideoData.VideoAuthor));
                cmd.Parameters.Add(new SqlParameter("@VideoBoughtDate", VideoData.VideoBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@VideoPublisher", VideoData.VideoPublisher));
                cmd.Parameters.Add(new SqlParameter("@VideoNote", VideoData.VideoNote));
                cmd.Parameters.Add(new SqlParameter("@VideoKeeperId", VideoData.VideoKeeperId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", VideoData.VideoStatusId));
                cmd.Parameters.Add(new SqlParameter("@ModifyUser", Environment.MachineName));
                cmd.Parameters.Add(new SqlParameter("@CreUsr", Environment.MachineName));
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran; //套用transaction
                try
                {
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    tran.Commit();
                    responseStatus.StatusCode = (int)HttpStatusCode.OK;
                    responseStatus.StatusMessage = "修改成功！";
                }
                catch (Exception)
                {
                    tran.Rollback();
                    responseStatus.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseStatus.StatusMessage = "修改失敗！";
                }
                finally
                {
                    conn.Close();
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// Map資料進借閱紀錄List
        /// </summary>
        /// <param name="VideoLendRecords">書本借閱資料</param>
        /// <returns>多筆借閱紀錄</returns>
        private List<Models.VideoLendRecord> MapVideoLendRecordToList(DataTable VideoLendRecords)
        {
            List<Models.VideoLendRecord> result = new List<VideoLendRecord>();
            foreach (DataRow row in VideoLendRecords.Rows)
            {
                result.Add(new VideoLendRecord()
                {
                    VideoLendDate = (DateTime)row["LendDate"],
                    KeeperId = row["KeeperId"]?.ToString(),
                    UserCname = row["UserCname"]?.ToString(),
                    UserEname = row["UserEname"]?.ToString()
                });
            }
            return result;
        }

        /// <summary>
        /// Map資料進書籍資料List
        /// </summary>
        /// <param name="VideoData"></param>
        /// <returns>多筆書籍資料</returns>
        private List<Models.VideoData> MapVideoDataSearchToList(DataTable VideoData)
        {
            List<Models.VideoData> result = new List<VideoData>();
            foreach (DataRow row in VideoData.Rows)
            {
                result.Add(new VideoData()
                {
                    VideoId = (int)row["VideoId"],
                    VideoName = row["VideoName"].ToString(),
                    VideoClassId = row["VideoClassId"].ToString(),
                    VideoClassName = row["VideoClassName"].ToString(),
                    VideoBoughtDate = (DateTime)row["VideoBoughtDate"],
                    VideoStatusId = row["VideoStatusId"].ToString(),
                    VideoStatusName = row["VideoStatusName"].ToString(),
                    VideoKeeperId = row["VideoKeeperId"].ToString(),
                    VideoKeeperName = row["VideoKeeperName"].ToString()
                });
            }
            return result;
        }
        private Models.VideoData MapVideoDataToList(DataTable VideoData)
        {
            Models.VideoData result = new VideoData();
            var row = VideoData.Rows[0];
            result = new VideoData()
            {
                VideoId = (int)row["VideoId"],
                VideoName = row["VideoName"].ToString(),
                VideoAuthor = row["VideoAuthor"].ToString(),
                VideoNote = row["VideoNote"].ToString(),
                VideoPublisher = row["VideoPublisher"].ToString(),
                VideoClassId = row["VideoClassId"].ToString(),
                VideoClassName = row["VideoClassName"].ToString(),
                VideoBoughtDate = (DateTime)row["VideoBoughtDate"],
                VideoStatusId = row["VideoStatusId"].ToString(),
                VideoStatusName = row["VideoStatusName"].ToString(),
                VideoKeeperId = row["VideoKeeperId"].ToString(),
                VideoKeeperName = row["VideoKeeperName"].ToString()
            };
            return result;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VideoManagement.Model;
using VideoManagement.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace VideoManagement.Dao
{
    public class VideoDataDao : IVideoDataDao
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
        /// 依照影片ID取得影片名稱
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>影片名稱</returns>
        public string GetVideoNameByVideoId(int videoId)
        {
            string VideoName = "";
            string sql = @"SELECT VIDEO_NAME AS VideoName
                           FROM VIDEO_DATA (NOLOCK)
                           WHERE VIDEO_DATA.VIDEO_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoId));
                VideoName = cmd.ExecuteScalar().ToString(); //取得第一列的第一行
                conn.Close();
            }

            return VideoName;
        }

        /// <summary>
        /// 依照影片類別ID判斷是否存在此影片類別
        /// </summary>
        /// <param name="videoClassId">影片類別ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoClassId(string videoClassId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM VIDEO_CLASS (NOLOCK) WHERE VIDEO_CLASS_ID = @VideoClassId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", videoClassId ?? string.Empty));
                result = cmd.ExecuteScalar() != null; //取得第一列的第一行
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 依照影片ID判斷是否存在此影片
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoId(int videoId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM VIDEO_DATA WHERE VIDEO_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoId));
                result = cmd.ExecuteScalar() != null; //取得第一列的第一行
                conn.Close();
            }

            return result;
        }

        /// <summary>
        /// 依照影片狀態ID判斷是否存在此狀態
        /// </summary>
        /// <param name="videoStatusId">影片狀態ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoStatusId(string videoStatusId)
        {
            bool result = false;
            string sql = @"SELECT 1 FROM VIDEO_CODE (NOLOCK) 
                           WHERE CODE_ID = @VideoStatusId AND CODE_TYPE = 'VIDEO_STATUS'";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", videoStatusId ?? string.Empty));
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
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
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
        /// 取得單一影片資料
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>單一影片資料</returns>
        public VideoData GetSingleVideoDataByVideoId(int videoId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.VIDEO_ID AS VideoId,
                                  bd.VIDEO_CLASS_ID AS VideoClassId,
	                              bcs.VIDEO_CLASS_NAME AS VideoClassName,
	                              bd.VIDEO_NAME AS VideoName,
	                              bd.VIDEO_BOUGHT_DATE AS VideoBoughtDate,
	                              bd.VIDEO_STATUS AS VideoStatusId,
	                              bcd.CODE_NAME AS VideoStatusName,
	                              bd.VIDEO_KEEPER AS VideoKeeperId,
	                              mm.USER_ENAME AS VideoKeeperName,
                                  bd.VIDEO_AUTHOR AS VideoAuthor,
                                  bd.VIDEO_PUBLISHER AS VideoPublisher,
                                  bd.VIDEO_NOTE AS VideoNote
                        FROM VIDEO_DATA AS bd
                        INNER JOIN VIDEO_CLASS AS bcs 
	                        ON bcs.VIDEO_CLASS_ID = bd.VIDEO_CLASS_ID
                        LEFT JOIN MEMBER_M AS mm 
	                        ON mm.USER_ID = bd.VIDEO_KEEPER
                        INNER JOIN VIDEO_CODE AS bcd 
	                        ON bcd.CODE_ID = bd.VIDEO_STATUS AND CODE_TYPE = 'VIDEO_STATUS'
                        WHERE bd.VIDEO_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //從Command取得資料存入dataAdapter
                sqlAdapter.Fill(dt); //將dataAdapter資料存入DataTable
                conn.Close();
            }

            return MapVideoDataToList(dt);
        }

        /// <summary>
        /// 依照條件取得影片資料
        /// </summary>
        /// <param name="arg">查詢參數</param>
        /// <returns>多筆影片資料</returns>
        public List<VideoData> GetVideoDataByCondtion(VideoDataSearchArg arg)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT bd.VIDEO_ID AS VideoId,
                                  bd.VIDEO_CLASS_ID AS VideoClassId,
	                              bcs.VIDEO_CLASS_NAME AS VideoClassName,
	                              bd.VIDEO_NAME AS VideoName,
	                              bd.VIDEO_BOUGHT_DATE AS VideoBoughtDate,
	                              bd.VIDEO_STATUS AS VideoStatusId,
	                              bcd.CODE_NAME AS VideoStatusName,
	                              bd.VIDEO_KEEPER AS VideoKeeperId,
	                              mm.USER_ENAME AS VideoKeeperName
                          FROM VIDEO_DATA AS bd (NOLOCK) 
                          INNER JOIN VIDEO_CLASS AS bcs (NOLOCK)  
	                          ON bcs.VIDEO_CLASS_ID = bd.VIDEO_CLASS_ID
                          LEFT JOIN MEMBER_M AS mm (NOLOCK)  
	                          ON mm.USER_ID = bd.VIDEO_KEEPER
                          INNER JOIN VIDEO_CODE AS bcd (NOLOCK)  
	                          ON bcd.CODE_ID = bd.VIDEO_STATUS  AND CODE_TYPE = 'VIDEO_STATUS'
                          WHERE (bd.VIDEO_NAME LIKE '%' + @VideoName + '%' OR @VideoName = '') AND
	                            (bd.VIDEO_CLASS_ID = @VideoClassId OR @VideoClassId = '')AND
	                            (bd.VIDEO_KEEPER = @VideoKeeperId OR @VideoKeeperId = '')AND
	                            (bd.VIDEO_STATUS = @VideoStatusId OR @VideoStatusId = '')
                          ORDER BY VideoBoughtDate DESC";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoName", CodeTool.DecodeStr(arg.VideoName) ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", CodeTool.DecodeStr(arg.VideoClassId) ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoKeeperId", CodeTool.DecodeStr(arg.VideoKeeperId) ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", CodeTool.DecodeStr(arg.VideoStatusId) ?? string.Empty));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd); //從Command取得資料存入dataAdapter
                sqlAdapter.Fill(dt); //將dataAdapter資料存入DataTable
                conn.Close();
            }

            return MapVideoDataSearchToList(dt);
        }

        /// <summary>
        /// 依照影片ID取得借閱資料
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>多筆借閱資料</returns>
        public List<VideoLendRecord> GetVideoLendDataByVideoId(int videoId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT BLR.LEND_DATE AS LendDate, BLR.KEEPER_ID AS KeeperId, 
	                              MM.USER_ENAME AS UserEname, MM.USER_CNAME AS UserCname
                           FROM VIDEO_LEND_RECORD AS BLR (NOLOCK) 
                           INNER JOIN MEMBER_M AS MM (NOLOCK) 
                               ON MM.USER_ID = BLR.KEEPER_ID
                           WHERE BLR.VIDEO_ID = @VideoId
                           ORDER BY LendDate DESC";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoId));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);//從Command取得資料存入dataAdapter
                sqlAdapter.Fill(dt);//將dataAdapter資料存入DataTable
                conn.Close();
            }

            return MapVideoLendRecordToList(dt);
        }

        /// <summary>
        /// 新增影片
        /// </summary>
        /// <param name="videoData">影片資料</param>
        /// <returns>新增狀態訊息</returns>
        public ResponseStatus InsertVideoData(VideoData videoData)
        {
            videoData = DecodeVideoData(videoData);
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @" INSERT INTO VIDEO_DATA
                            (
	                            VIDEO_NAME, VIDEO_CLASS_ID, VIDEO_AUTHOR,
	                            VIDEO_BOUGHT_DATE, VIDEO_PUBLISHER, VIDEO_NOTE,
	                            VIDEO_STATUS, CREATE_DATE, CREATE_USER
                            )
                            VALUES
                            (
	                            @VideoName, @VideoClassId, @VideoAuthor, 
	                            @VideoBoughtDate, @VideoPublisher, @VideoNote,
	                            'A', GETDATE(), @CreateUser
                            )";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoName", videoData.VideoName));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", videoData.VideoClassId));
                cmd.Parameters.Add(new SqlParameter("@VideoAuthor", videoData.VideoAuthor));
                cmd.Parameters.Add(new SqlParameter("@VideoBoughtDate", videoData.VideoBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@VideoPublisher", videoData.VideoPublisher));
                cmd.Parameters.Add(new SqlParameter("@VideoNote", videoData.VideoNote));
                cmd.Parameters.Add(new SqlParameter("@CreateUser", Environment.MachineName));
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    responseStatus.StatusCode = true; //要改
                    responseStatus.StatusMessage = "新增成功！";
                }
                catch (Exception ex)
                {
                    responseStatus.StatusCode = false;
                    responseStatus.StatusMessage = "新增失敗！";
                    Logger.Write(Logger.LogCategory.Error,ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// 刪除影片
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>刪除狀態訊息</returns>
        public ResponseStatus DeleteVideoData(int videoId)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @"DELETE FROM VIDEO_DATA 
                           WHERE (VIDEO_DATA.VIDEO_ID = @VideoId)";

            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoId));
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    responseStatus.StatusCode = true;
                    responseStatus.StatusMessage = "刪除成功！";
                }
                catch (Exception ex)
                {
                    responseStatus.StatusCode = false;
                    responseStatus.StatusMessage = "刪除失敗！";
                    Logger.Write(Logger.LogCategory.Error, ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// 修改影片
        /// </summary>
        /// <param name="videoData">影片資料</param>
        /// <returns>修改狀態訊息</returns>
        public ResponseStatus UpdateVideoData(VideoData videoData)
        {
            videoData = DecodeVideoData(videoData);
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @" UPDATE VIDEO_DATA 
		                    SET VIDEO_NAME = @VideoName, VIDEO_CLASS_ID = @VideoClassId,
		                    VIDEO_AUTHOR = @VideoAuthor, VIDEO_BOUGHT_DATE = @VideoBoughtDate,
		                    VIDEO_PUBLISHER = @VideoPublisher, VIDEO_NOTE = @VideoNote,
		                    VIDEO_KEEPER = @VideoKeeperId, VIDEO_STATUS = @VideoStatusId, 
		                    MODIFY_DATE = GETDATE(), MODIFY_USER = @ModifyUser
		                    WHERE VIDEO_DATA.VIDEO_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoData.VideoId));
                cmd.Parameters.Add(new SqlParameter("@VideoName", videoData.VideoName));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", videoData.VideoClassId));
                cmd.Parameters.Add(new SqlParameter("@VideoAuthor", videoData.VideoAuthor));
                cmd.Parameters.Add(new SqlParameter("@VideoBoughtDate", videoData.VideoBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@VideoPublisher", videoData.VideoPublisher));
                cmd.Parameters.Add(new SqlParameter("@VideoNote", videoData.VideoNote));
                cmd.Parameters.Add(new SqlParameter("@VideoKeeperId", videoData.VideoKeeperId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", videoData.VideoStatusId));
                cmd.Parameters.Add(new SqlParameter("@ModifyUser", Environment.MachineName));
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    responseStatus.StatusCode = true;
                    responseStatus.StatusMessage = "修改成功！";
                }
                catch (Exception ex)
                {
                    responseStatus.StatusCode = false;
                    responseStatus.StatusMessage = "修改失敗！";
                    Logger.Write(Logger.LogCategory.Error, ex.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// 修改影片與新增借閱紀錄
        /// </summary>
        /// <param name="videoData">影片資料</param>
        /// <returns>修改狀態訊息</returns>
        public ResponseStatus UpdateVideoDataAndLendRecord(VideoData videoData)
        {
            videoData = DecodeVideoData(videoData);
            ResponseStatus responseStatus = new ResponseStatus();
            string sql = @" INSERT INTO VIDEO_LEND_RECORD
			                (
				                VIDEO_ID, KEEPER_ID, LEND_DATE, CRE_DATE, CRE_USR
			                )
			                VALUES
			                (
				                @VideoId, @VideoKeeperId, GETDATE(), GETDATE(), @CreUsr
			                );
		                    UPDATE VIDEO_DATA 
		                    SET VIDEO_NAME = @VideoName, VIDEO_CLASS_ID = @VideoClassId,
		                    VIDEO_AUTHOR = @VideoAuthor, VIDEO_BOUGHT_DATE = @VideoBoughtDate,
		                    VIDEO_PUBLISHER = @VideoPublisher, VIDEO_NOTE = @VideoNote,
		                    VIDEO_KEEPER = @VideoKeeperId, VIDEO_STATUS = @VideoStatusId, 
		                    MODIFY_DATE = GETDATE(), MODIFY_USER = @ModifyUser
		                    WHERE VIDEO_DATA.VIDEO_ID = @VideoId";
            using (SqlConnection conn = new SqlConnection(GetDBConnectionString()))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@VideoId", videoData.VideoId));
                cmd.Parameters.Add(new SqlParameter("@VideoName", videoData.VideoName));
                cmd.Parameters.Add(new SqlParameter("@VideoClassId", videoData.VideoClassId));
                cmd.Parameters.Add(new SqlParameter("@VideoAuthor", videoData.VideoAuthor));
                cmd.Parameters.Add(new SqlParameter("@VideoBoughtDate", videoData.VideoBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@VideoPublisher", videoData.VideoPublisher));
                cmd.Parameters.Add(new SqlParameter("@VideoNote", videoData.VideoNote));
                cmd.Parameters.Add(new SqlParameter("@VideoKeeperId", videoData.VideoKeeperId ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@VideoStatusId", videoData.VideoStatusId));
                cmd.Parameters.Add(new SqlParameter("@ModifyUser", Environment.MachineName));
                cmd.Parameters.Add(new SqlParameter("@CreUsr", Environment.MachineName));
                conn.Open();
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran; //套用transaction
                try
                {
                    cmd.ExecuteNonQuery(); //用來執行INSERT、UPDATE、DELETE和其他沒有返回值得SQL命令。
                    tran.Commit();
                    responseStatus.StatusCode = true;
                    responseStatus.StatusMessage = "修改成功！新增了一筆借閱紀錄。";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    responseStatus.StatusCode = false;
                    responseStatus.StatusMessage = "修改失敗！";
                    Logger.Write(Logger.LogCategory.Error, ex.ToString());
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
        /// <param name="videoLendRecords">影片借閱資料</param>
        /// <returns>多筆借閱紀錄</returns>
        private List<VideoLendRecord> MapVideoLendRecordToList(DataTable videoLendRecords)
        {
            List<VideoLendRecord> result = new List<VideoLendRecord>();
            foreach (DataRow row in videoLendRecords.Rows)
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
        /// Map資料進影片資料List
        /// </summary>
        /// <param name="videoData"></param>
        /// <returns>多筆影片資料</returns>
        private List<VideoData> MapVideoDataSearchToList(DataTable videoData)
        {
            List<VideoData> result = new List<VideoData>();
            foreach (DataRow row in videoData.Rows)
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
        private VideoData MapVideoDataToList(DataTable videoData)
        {
            VideoData result = new VideoData();
            if(videoData.Rows.Count != 0)
            {
                var row = videoData.Rows[0];
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
            }
            
            return result;
        }

        /// <summary>
        /// Decode
        /// </summary>
        /// <param name="videoData"></param>
        /// <returns></returns>
        private static VideoData DecodeVideoData(VideoData videoData)
        {
            VideoData result = new VideoData()
            {
                VideoId = videoData.VideoId,
                VideoName = CodeTool.DecodeStr(videoData.VideoName),
                VideoAuthor = CodeTool.DecodeStr(videoData.VideoAuthor),
                VideoNote = CodeTool.DecodeStr(videoData.VideoNote),
                VideoPublisher = CodeTool.DecodeStr(videoData.VideoPublisher),
                VideoClassId = videoData.VideoClassId,
                VideoBoughtDate = videoData.VideoBoughtDate,
                VideoStatusId = videoData.VideoStatusId,
                VideoKeeperId = videoData.VideoKeeperId,
            };
            return result;
        }
    }
}

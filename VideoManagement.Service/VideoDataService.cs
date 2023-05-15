using VideoManagement.Dao;
using VideoManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoManagement.Service
{
    public class VideoDataService : IVideoDataService
    {
        private IVideoDataDao videoDataDao { get; set; }
        /// <summary>
        /// 依照影片ID取得影片名稱
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>影片名稱</returns>
        public string GetVideoNameByVideoId(int videoId)
        {
            return videoDataDao.GetVideoNameByVideoId(videoId);
        }

        // <summary>
        /// 依照影片類別ID判斷是否存在此影片類別
        /// </summary>
        /// <param name="videoClassId">影片類別ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoClassId(string videoClassId)
        {
            return videoDataDao.IsExistVideoClassId(videoClassId);
        }

        /// <summary>
        /// 依照影片ID判斷是否存在此影片
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoId(int videoId)
        {
            return videoDataDao.IsExistVideoId(videoId);
        }

        /// <summary>
        /// 依照影片狀態ID判斷是否存在此狀態
        /// </summary>
        /// <param name="videoStatusId">影片狀態ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistVideoStatusId(string videoStatusId)
        {
            return videoDataDao.IsExistVideoStatusId(videoStatusId);
        }

        /// <summary>
        /// 依照人員ID判斷是否存在此人員
        /// </summary>
        /// <param name="userId">借閱人ID</param>
        /// <returns>是否存在</returns>
        public bool IsExistMemberId(string userId)
        {
            return videoDataDao.IsExistMemberId(userId);
        }
        /// <summary>
        /// 取得單一影片資料
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>單一影片資料</returns>
        public VideoData GetSingleVideoDataByVideoId(int videoId)
        {
            return videoDataDao.GetSingleVideoDataByVideoId(videoId);
        }

        /// <summary>
        /// 依照條件取得影片資料
        /// </summary>
        /// <param name="arg">查詢參數</param>
        /// <returns>多筆影片資料</returns>
        public List<VideoData> GetVideoDataByCondtion(VideoDataSearchArg arg)
        {
            return videoDataDao.GetVideoDataByCondtion(arg);
        }
        /// <summary>
        /// 依照影片ID取得借閱資料
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>多筆借閱資料</returns>
        public List<VideoLendRecord> GetVideoLendDataByVideoId(int videoId)
        {
            return videoDataDao.GetVideoLendDataByVideoId(videoId);
        }

        /// <summary>
        /// 新增影片
        /// </summary>
        /// <param name="videoData">影片資料</param>
        /// <returns>新增狀態訊息</returns>
        public ResponseStatus InsertVideoData(VideoData videoData)
        {
            return videoDataDao.InsertVideoData(videoData);
        }

        /// <summary>
        /// 刪除影片
        /// </summary>
        /// <param name="videoId">影片ID</param>
        /// <returns>刪除狀態訊息</returns>
        public ResponseStatus DeleteVideoData(int videoId)
        {
            return videoDataDao.DeleteVideoData(videoId);
        }

        /// <summary>
        /// 新增影片
        /// </summary>
        /// <param name="videoData">影片資料</param>
        /// <returns>修改狀態訊息</returns>
        public ResponseStatus UpdateVideoData(VideoData videoData)
        {
            return videoDataDao.UpdateVideoData(videoData);
        }

        /// <summary>
        /// 修改影片與新增借閱紀錄
        /// </summary>
        /// <param name="videoData">影片資料</param>
        /// <returns>修改狀態訊息</returns>
        public ResponseStatus UpdateVideoDataAndLendRecord(VideoData videoData)
        {
            return videoDataDao.UpdateVideoDataAndLendRecord(videoData);
        }
    }
}

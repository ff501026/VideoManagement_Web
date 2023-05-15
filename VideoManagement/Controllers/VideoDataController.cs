using System.Web.Mvc;
using VideoManagement.Service;
using VideoManagement.Model;

namespace VideoManagement.Controllers
{
    public class VideoDataController : Controller
    {
        public IVideoDataService videoDataService { get; set; }
        public IDropDownListService dropDownListService { get; set; }

        /// <summary>
        /// 影片資料查詢畫面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得影片類別下拉選單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetVideoClassDropDown()
        {
            return Json(dropDownListService.GetVideoClassId());
        }

        /// <summary>
        /// 取得影片狀態下拉選單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetVideoStatusDropDown()
        {
            return Json(dropDownListService.GetVideoStatus("VIDEO_STATUS"));
        }

        /// <summary>
        /// 取得借閱人下拉選單
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetVideoKeeperDropDown()
        {
            return Json(dropDownListService.GetMemberMId());
        }

        /// <summary>
        /// 影片資料查詢
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult GetVideoDataByCondition(VideoDataSearchArg arg)
        {
            return Json(videoDataService.GetVideoDataByCondtion(arg));
        }

        /// <summary>
        /// 新增影片畫面
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult InsertVideoData()
        {
            return View();
        }

        /// <summary>
        /// 新增影片
        /// </summary>
        /// <param name="videoData"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult InsertVideoData(VideoData videoData)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            //驗證有沒有此影片類別
            if (!videoDataService.IsExistVideoClassId(videoData.VideoClassId))
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "新增失敗！請選擇正確的影片類別。";
                return Json(responseStatus);
            }
            else if (ModelState.IsValid) //表單驗證成功
            {
                //新增資料
                responseStatus = videoDataService.InsertVideoData(videoData);
                return Json(responseStatus);
            }
            responseStatus.StatusCode = false;
            responseStatus.StatusMessage = "新增失敗！請確認是否有正確填寫。";
            return Json(responseStatus);
        }

        /// <summary>
        /// 刪除影片
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteVideoData(int videoId)
        {
            ResponseStatus responseStatus = new ResponseStatus();

            if (!videoDataService.IsExistVideoId(videoId)) //驗證有沒有此影片 First
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "刪除失敗！請確認此影片是否存在。";
                return Json(responseStatus);
            }
            //取得VideoData的資料
            VideoData DefaultvideoData = videoDataService.GetSingleVideoDataByVideoId(videoId);
            //驗證是否為已借出
            if (DefaultvideoData.VideoStatusId.Equals("B") || DefaultvideoData.VideoStatusId.Equals("C")) 
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "刪除失敗！影片外借中無法刪除。";
                return Json(responseStatus);
            }
            //刪除影片
            responseStatus = videoDataService.DeleteVideoData(videoId);
            return Json(responseStatus);
        }
        /// <summary>
        /// 修改影片畫面
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UpdateVideoData(int videoId)
        {
            return View(videoId);
        }
        /// <summary>
        /// 修改影片
        /// </summary>
        /// <param name="videoData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateVideoData(VideoData videoData)
        {
            ResponseStatus responseStatus = IsIdExist(videoData);
            if (!responseStatus.StatusCode)
            {
                return Json(responseStatus);
            }
            //表單驗證成功執行
            if (ModelState.IsValid)
            {
                //驗證有沒有此影片
                if (!videoDataService.IsExistVideoId(videoData.VideoId))
                {
                    responseStatus.StatusCode = false;
                    responseStatus.StatusMessage = "修改失敗！請確認此影片是否存在。";
                    return Json(responseStatus);
                }
                if (videoData.VideoStatusId.Equals("B") || videoData.VideoStatusId.Equals("C"))
                {
                    var oldVideoData = videoDataService.GetSingleVideoDataByVideoId(videoData.VideoId);
                    if (oldVideoData.VideoStatusId != videoData.VideoStatusId ||
                        oldVideoData.VideoKeeperId != videoData.VideoKeeperId)
                    {
                        //更新資料並新增借閱紀錄
                        responseStatus = videoDataService.UpdateVideoDataAndLendRecord(videoData);
                        return Json(responseStatus);
                    }
                    else
                    {
                        //更新資料
                        responseStatus = videoDataService.UpdateVideoData(videoData);
                        return Json(responseStatus);
                    }
                }
                else
                {
                    //更新資料
                    responseStatus = videoDataService.UpdateVideoData(videoData);
                    return Json(responseStatus);
                }
                
            }
            responseStatus.StatusCode = false;
            responseStatus.StatusMessage = "修改失敗！請確認是否有正確填寫。";
            return Json(responseStatus);
        }
        /// <summary>
        /// 取得單筆影片資料
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetSingleVideoDataByVideoId(int videoId)
        {
            var result = videoDataService.GetSingleVideoDataByVideoId(videoId);
            if (result.VideoId == 0) 
            {
                return Json(new { StatusCode = false});
            }
            return Json(new {StatusCode = true, Video = result });
        }
        

        /// <summary>
        /// 影片明細畫面
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult VideoDetail(int videoId)
        {
            return View("UpdateVideoData", videoId);
        }

        /// <summary>
        /// 借閱紀錄畫面
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetVideoLendData(int videoId)
        {
            return View(videoId);
        }

        /// <summary>
        /// 查詢影片名稱
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetVideoNameByVideoId(int videoId)
        {
            ResponseStatus responseStatus = new ResponseStatus();
            //驗證有沒有此影片
            if (!videoDataService.IsExistVideoId(videoId))
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "請確認是否存在此影片。";
                return Json(responseStatus);
            }
            return Json(new { StatusCode = true, Name = videoDataService.GetVideoNameByVideoId(videoId) });
        }

        /// <summary>
        /// 查詢借閱紀錄
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetVideoLendDataByVideoId(int videoId)
        {
            return Json(videoDataService.GetVideoLendDataByVideoId(videoId));
        }

        private ResponseStatus IsIdExist(VideoData videoData)
        {
            ResponseStatus responseStatus = new ResponseStatus{ 
                StatusCode = true,
            };
            //驗證有沒有此影片類別
            if (!videoDataService.IsExistVideoClassId(videoData.VideoClassId))
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "請選擇正確的影片類別。";
            }
            else if (!videoDataService.IsExistVideoStatusId(videoData.VideoStatusId))//驗證有沒有此借閱狀態
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "請選擇正確的借閱狀態。";
            }
            else if ((videoData.VideoStatusId.Equals("B") || videoData.VideoStatusId.Equals("C")) &&
                !videoDataService.IsExistMemberId(videoData.VideoKeeperId))
            {
                responseStatus.StatusCode = false;
                responseStatus.StatusMessage = "請選擇正確的借閱人。";
            } // if elseif 
            return responseStatus;
        }
    }
}
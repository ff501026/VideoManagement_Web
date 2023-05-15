using VideoManagement.Model;
using System.Collections.Generic;

namespace VideoManagement.Dao
{
    public interface IVideoDataDao
    {
        ResponseStatus DeleteVideoData(int videoId);
        List<VideoData> GetVideoDataByCondtion(VideoDataSearchArg arg);
        List<VideoLendRecord> GetVideoLendDataByVideoId(int videoId);
        string GetVideoNameByVideoId(int videoId);
        VideoData GetSingleVideoDataByVideoId(int videoId);
        ResponseStatus InsertVideoData(VideoData videoData);
        bool IsExistVideoClassId(string videoClassId);
        bool IsExistVideoId(int videoId);
        bool IsExistVideoStatusId(string videoStatusId);
        bool IsExistMemberId(string userId);
        ResponseStatus UpdateVideoData(VideoData videoData);
        ResponseStatus UpdateVideoDataAndLendRecord(VideoData videoData);
    }
}
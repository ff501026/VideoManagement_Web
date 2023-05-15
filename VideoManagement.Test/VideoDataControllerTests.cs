using VideoManagement.Controllers;
using VideoManagement.Model;
using VideoManagement.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;

namespace VideoManagement.Test
{
    [TestClass]
    public class VideoDataControllerTests
    {
        /// <summary>
        /// 測試刪除邏輯(可以借出/不可借出)
        /// </summary>
        /// <param name="VideoStatusId"></param>
        [TestMethod]
        [TestCategory("DeleteVideoData")]
        [DataRow("A")]
        [DataRow("U")]
        public void DeleteVideoWhenStatusIsA(string VideoStatusId)
        {
            // Arranage
            int videoId = 2;

            Mock<IVideoDataService> mockVideoDataService = new Mock<IVideoDataService>();
            mockVideoDataService.Setup(mock => mock.GetSingleVideoDataByVideoId(It.Is<int>(VideoId => VideoId == videoId))) //自訂義VideoData的結果
                .Returns(new VideoData
                {
                    VideoId = videoId,
                    VideoStatusId = VideoStatusId
                });
            mockVideoDataService.Setup(mock => mock.IsExistVideoId(It.Is<int>(VideoId => VideoId == videoId))) //自訂義影片存在
                .Returns(true);
            mockVideoDataService.Setup(mock => mock.DeleteVideoData(It.Is<int>(VideoId => VideoId == videoId))) //自訂義刪除成功
                .Returns(new ResponseStatus
                {
                    StatusCode = true,
                    StatusMessage = "刪除成功！"
                });

            var controller = new VideoDataController()
            {
                videoDataService = mockVideoDataService.Object
            };

            // Act
            var result = controller.DeleteVideoData(videoId); //A會進入DeleteVideoData得到刪除成功

            // Assert
            ResponseStatus data = (ResponseStatus)result.Data;
            Assert.IsTrue(data.StatusCode); //結果要為true，結果要放前面
        }

        /// <summary>
        /// 測試刪除邏輯(已借出/已借出未領)
        /// </summary>
        /// <param name="VideoStatusId"></param>
        [TestMethod]
        [TestCategory("DeleteVideoData")]
        [DataRow("B")]
        [DataRow("C")]
        public void DeleteVideoWhenStatusIsB(string VideoStatusId)
        {
            // Arranage
            int videoId = 1;
            Mock<IVideoDataService> mockVideoDataService = new Mock<IVideoDataService>();
            mockVideoDataService.Setup(mock => mock.GetSingleVideoDataByVideoId(It.IsAny<int>())) //自訂義VideoData的結果
                .Returns(new VideoData
                {
                    VideoId = videoId,
                    VideoStatusId = VideoStatusId
                });
            mockVideoDataService.Setup(mock => mock.IsExistVideoId(It.IsAny<int>())) //自訂義影片存在
                .Returns(true);
            mockVideoDataService.Setup(mock => mock.DeleteVideoData(It.IsAny<int>())) //自訂義刪除成功
                .Returns(new ResponseStatus
                {
                    StatusCode = true,
                    StatusMessage = "刪除成功！"
                });

            var controller = new VideoDataController()
            {
                videoDataService = mockVideoDataService.Object
            };

            // Act
            var result = controller.DeleteVideoData(videoId); //B不會進到DeleteVideoData所以不會拿到true

            // Assert
            ResponseStatus data = (ResponseStatus)result.Data;
            Assert.IsFalse(data.StatusCode); //結果要為false
        }

        /// <summary>
        /// 測試借閱邏輯
        /// </summary>
        /// <param name="VideoStatusId"></param>
        [TestMethod]
        [TestCategory("UpdateVideoData")]
        [DataRow("B")]
        [DataRow("C")]
        public void UpdateVideoWhenStatusIsB(string VideoStatusId)
        {
            //Arrange
            int videoId = 1;
            VideoData oldVideoData = new VideoData //自訂義原本的狀態是B 借書人是0002
            {
                VideoId = videoId,
                VideoStatusId = VideoStatusId,
                VideoKeeperId = "0002"
            };
            VideoData newVideoData = new VideoData //自訂義新的狀態是B 借書人是0001
            {
                VideoId = videoId,
                VideoName = "嗨",
                VideoAuthor = "嗨",
                VideoNote = "嗨",
                VideoPublisher = "嗨",
                VideoClassId = "BK",
                VideoBoughtDate = DateTime.Now,
                VideoStatusId = VideoStatusId,
                VideoKeeperId = "0001",
            };
            Mock<IVideoDataService> mockVideoDataService = new Mock<IVideoDataService>();
            mockVideoDataService.Setup(mock => mock.IsExistVideoClassId(It.IsAny<string>())) //自訂義影片類別存在
                .Returns(true);
            mockVideoDataService.Setup(mock => mock.IsExistVideoStatusId(It.IsAny<string>())) //自訂義借閱狀態存在
                .Returns(true);
            mockVideoDataService.Setup(mock => mock.IsExistMemberId(It.IsAny<string>())) //自訂義借閱人存在
                .Returns(true);
            mockVideoDataService.Setup(mock => mock.IsExistVideoId(It.IsAny<int>())) //自訂義影片存在
               .Returns(true);
            mockVideoDataService.Setup(mock => mock.GetSingleVideoDataByVideoId(It.IsAny<int>())) //自訂義原本的書
                 .Returns(oldVideoData);
            mockVideoDataService.Setup(mock => mock.UpdateVideoDataAndLendRecord(It.IsAny<VideoData>())) //自訂義修改結果
                .Returns(new ResponseStatus
                {
                    StatusCode = true,
                    StatusMessage = "修改成功！新增了一筆借閱紀錄。"
                });
            mockVideoDataService.Setup(mock => mock.UpdateVideoData(It.Is<VideoData>(VideoData => VideoData == newVideoData)))
                .Returns(new ResponseStatus
                {
                    StatusCode = true,
                    StatusMessage = "修改成功！"
                });
            var controller = new VideoDataController()
            {
                videoDataService = mockVideoDataService.Object
            };
            // Act
            var result = controller.UpdateVideoData(newVideoData);
            ResponseStatus responseStatus = (ResponseStatus)result.Data;
            //Assert
            Assert.AreEqual("修改成功！新增了一筆借閱紀錄。",responseStatus.StatusMessage);

        }


    }
}

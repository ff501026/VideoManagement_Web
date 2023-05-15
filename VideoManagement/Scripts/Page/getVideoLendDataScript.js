$(function () {
    $(document).attr("title", "影片明細");

    var getUrlString = location.href; //利用location.href取得網址
    var url = new URL(getUrlString); //將網址 (字串轉成URL)
    var videoId = url.searchParams.get('videoId'); // 使用URL.searchParams + get 函式，取得結果的KEY鍵值參數
    $.ajax({
        dataType: 'json',
        type: 'post',
        url: "/VideoData/GetVideoNameByVideoId",
        data: "videoId=" + videoId,
        success: function (response) {
            if (response.StatusCode) {
                $('#videoName').text(response.Name);
            } else {
                $("<div></div>").kendoDialog({
                    content: "查無此書！",
                    close: function (e) {
                        window.location.href = "/videoData/index"; //回到首頁
                    },
                    actions: [{
                        text: "確認",
                        action: function (e) {
                            window.location.href = "/videoData/index"; //找不到就回到首頁
                        }
                    }]
                }).data("kendoDialog").open().center;
            }
        }, error: function () {
            window.location.href = "/Shared/Error"
        }
    });
    $("#lendRecordGrid").kendoGrid({ //影片表格
        dataSource: {
            transport: {
                read: {
                    dataType: 'json',
                    type: 'post',
                    url: "/VideoData/GetVideoLendDataByVideoId",
                    data: function () {
                        return {
                            videoId: videoId
                        };
                    },
                }
            },
            pageSize: 20, //最多顯示幾筆
        },
        height: 550,
        sortable: true, //開啟排序功能
        pageable: {
            input: true,
            numeric: false, //頁碼選擇器
            messages: {
                display: "顯示第 {0} 到第 {1}項記錄，總共 {2} 項記錄",
                empty: "沒有找到符合的結果",
                page: "第",
                of: "頁，總共 {0} 頁"
            }
        }, //分頁
        columns: [ //欄位配置
            { field: "VideoLendDate", type: "date", format: "{0:yyyy/MM/dd}", title: "借閱日期" },
            { field: "KeeperId", title: "借閱人員編號" },
            { field: "UserEname", title: "英文姓名" }, //field取dataSource裡對應的Key
            { field: "UserCname", title: "中文姓名" }
        ]

    });
});
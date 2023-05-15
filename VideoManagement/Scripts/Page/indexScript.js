$(function () {
    $(document).attr("title", "影片維護系統");

    $("#videoName").kendoTextBox({
        placeholder: "請輸入影片名稱"
    });
    $("#videoClassId").kendoDropDownList({
        dataTextField: "text", //顯示的字
        dataValueField: "value", //值
        dataSource: {  //資料來源
            transport: {
                read: {
                    url: "/VideoData/GetVideoClassDropDown",
                    dataType: "json",
                    type: "post"
                }
            }
        },
        optionLabel: "請選擇",
        index: 0 //預設在哪個選項
    });//影片種類下拉選單

    $("#videoKeeperId").kendoDropDownList({
        dataTextField: "text", //顯示的字
        dataValueField: "value", //值
        dataSource: {  //資料來源
            transport: {
                read: {
                    url: "/VideoData/GetVideoKeeperDropDown",
                    dataType: "json",
                    type: "post"
                }
            }
        },
        optionLabel: "請選擇",
        index: 0 //預設在哪個選項
    });//借閱人下拉選單

    $("#videoStatusId").kendoDropDownList({
        dataTextField: "text", //顯示的字
        dataValueField: "value", //值
        dataSource: {  //資料來源
            transport: {
                read: {
                    url: "/VideoData/GetVideoStatusDropDown",
                    dataType: "json",
                    type: "post"
                }
            }
        },
        optionLabel: "請選擇",
        index: 0 //預設在哪個選項
    });//影片狀態下拉選單

    $("#videoGrid").kendoGrid({ //影片表格
        dataSource: {
            transport: {
                read: {
                    url: "/VideoData/GetVideoDataByCondition",
                    dataType: "json",
                    type: "post",
                    data: function () {
                        return {
                            VideoName: EncodeHtml($("#videoName").data("kendoTextBox").value()),
                            VideoClassId: EncodeHtml($("#videoClassId").data("kendoDropDownList").value()),
                            VideoKeeperId: EncodeHtml($("#videoKeeperId").data("kendoDropDownList").value()),
                            VideoStatusId: EncodeHtml($("#videoStatusId").data("kendoDropDownList").value())
                        }
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
                of: "頁　總共 {0} 頁"
            }
        }, //分頁
        columns: [ //欄位配置
            { hidden: true, field: "VideoId" },
            { field: "VideoClassName", title: "影片類別", width: "12%" }, //field取dataSource裡對應的Key
            { field: "VideoName", template: '<a href="/VideoData/VideoDetail?videoId=${VideoId}">${VideoName}</a>', title: "影片名稱" },
            { field: "VideoBoughtDate", type: "date", format: "{0:yyyy/MM/dd}", title: "影片購買日期", width: "12%" }, //了解format 
            { field: "VideoStatusName", title: "借閱狀態", width: "10%" },
            { field: "VideoKeeperName", title: "借閱人", width: "10%" },
            { command: { name: "btnGetVideoLend", text: "借閱紀錄", click: videoLendRecord, className: "btn-default" }, width: "120px" },
            { command: { name: "btnUpdate", text: "編輯", click: videoUpdate, className: "btn-info" }, width: "90px" },
            { command: { name: "btnDelete", text: "刪除", click: videoDelete, className: "btn-danger" }, width: "100px" }
        ]

    });
    
    $("#btnClear").click(function () {
        window.location.href = "/VideoData/Index" //回到首頁
    });
    $("#btnAdd").click(function () {
        window.location.href = "/VideoData/InsertVideoData" //回到新增頁面
    });
    $("#btnSearch").click(function () {
        $("#videoGrid").data("kendoGrid").dataSource.read();
    });

});

//功能:進入借閱紀錄畫面
function videoLendRecord(e) {
    e.preventDefault(); //取消事件的預設行為
    var tr = $(e.target).closest("tr"); //e.target：指的是觸發事件的物件。 closest：返回被選元素的第一個祖先元素
    var videoId = tr.find("td:eq(0)").text(); //取得同列第一欄的text
    window.location.href = "/VideoData/GetVideoLendData?videoId=" + videoId; //回到首頁
}

//功能:進入影片編輯畫面
function videoUpdate(e) {
    e.preventDefault(); //取消事件的預設行為
    var tr = $(e.target).closest("tr"); //e.target：指的是觸發事件的物件。 closest：返回被選元素的第一個祖先元素
    var videoId = tr.find("td:eq(0)").text(); //取得同列第一欄的text
    window.location.href = "/VideoData/UpdateVideoData?videoId=" + videoId; //回到首頁
}

//功能:刪除影片
function videoDelete(e) {
    e.preventDefault(); //取消事件的預設行為
    var grid = $("#videoGrid").data("kendoGrid");
    var tr = $(e.target).closest("tr"); //e.target：指的是觸發事件的物件。 closest：返回被選元素的第一個祖先元素
    var confirmText = "確定要刪除這本書嗎?";
    $("<div></div>").kendoConfirm({
        content: confirmText,
        actions: [{
            text: "確認",
            action: function (e) {
                $.ajax({
                    url: "/VideoData/DeleteVideoData",
                    type: "post",
                    data: "videoId=" + tr.find("td:eq(0)").text(),
                    dataType: "json",
                    success: function (response) {
                        if (response.StatusCode) {
                            //grid.removeRow(tr); //刪除指定的行，同時刪除dataSource裡對應的數據。
                            grid.dataSource.read();
                            showAlert('alert-success', response.StatusMessage);
                        } else {
                            showAlert('alert-danger', response.StatusMessage);
                        }
                    }, error: function (error) {
                        window.location.href = "/Shared/Error"
                    }
                });
                return true;
            },
            primary: true //按鈕顏色
        }, {
            text: "取消"
        }]
    }).data("kendoConfirm").open();
}
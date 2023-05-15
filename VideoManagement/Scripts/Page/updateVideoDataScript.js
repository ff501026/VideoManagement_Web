var getUrlString = location.href; //利用location.href取得網址
var url = new URL(getUrlString); //將網址 (字串轉成URL)
var pathName = url.pathname; //取得網址路徑
var videoId = url.searchParams.get('videoId'); // 取得videoId
$(function () {
    $(document).attr("title", "影片維護");

    $.ajax({
        dataType: 'json',
        type: 'post',
        url: "/VideoData/GetSingleVideoDataByVideoId", //取得影片資料
        data: "videoId=" + videoId,
        success: function (response) {
            if (response.StatusCode) {
                var video = response.Video;
                $('#videoName').data("kendoTextBox").value(video.VideoName);
                $('#videoAuthor').data("kendoTextBox").value(video.VideoAuthor);
                $('#videoPublisher').data("kendoTextBox").value(video.VideoPublisher);
                $('#videoNote').data("kendoTextArea").value(video.VideoNote);
                $("#videoBoughtDate").data("kendoDatePicker").value(video.VideoBoughtDate);
                $("#videoClassId").data("kendoDropDownList").value(video.VideoClassId);
                if (pathName !== "/VideoData/VideoDetail") {
                    $("#videoStatusId").data("kendoDropDownList").value(video.VideoStatusId);
                    $("#videoKeeperId").data("kendoDropDownList").value(video.VideoKeeperId);
                }
                setBtnKeeper(); //控制借閱人的欄位是否可填寫
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
            window.location.href = "/Shared/Error" //找不到書就跳到Error畫面
        }
    });
    $("#videoName").kendoTextBox({
        placeholder: "請輸入影片名稱"
    });
    $("#videoAuthor").kendoTextBox({
        placeholder: "請輸入作者"
    });
    $("#videoPublisher").kendoTextBox({
        placeholder: "請輸入出版商"
    });
    $("#videoNote").kendoTextArea({
        placeholder: "請輸入內容簡介",
        rows: 10,
    });
    $("#videoBoughtDate").kendoDatePicker({
        culture: "zh-TW",
        format: "yyyy/MM/dd", //設定DatePicker的格式
        max: new Date(), //設定最大不能超過今天
        min: new Date(1752,12,1)
    }); //影片購買日期的日期選擇器

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
        index: 0 //預設在哪個選項
    });//影片狀態下拉選單
    if (pathName === "/VideoData/VideoDetail") {
        $('#subTitle').text("影片明細");
        $(":input").attr("readonly", "readonly"); //:input選擇所有表單elements(input, select, textarea, button)
        $("#videoBoughtDate").data("kendoDatePicker").enable(false);
        $("#videoClassId").data("kendoDropDownList").enable(false);
        $("#videoStatusId").data("kendoDropDownList").enable(false);
        $("#videoKeeperId").data("kendoDropDownList").enable(false);
        $("#divButton").empty(); //把放button的div清空
        $("#divvideoKeeper").empty(); //把放button的div清空
        $("#divvideoStatus").empty(); //把放button的div清空
    } else {
        $('#subTitle').text("影片維護");
        $("#divButton").removeClass("hide");
        $("#btnClear").hide();
        $('#videoStatusId').change(function () {
            showBtnClear();
            setBtnKeeper();
        });
        $('#videoName').on("input", showBtnClear);
        $('#videoAuthor').on("input", showBtnClear);
        $('#videoPublisher').on("input", showBtnClear);
        $('#videoNote').on("input", showBtnClear);
        $('#videoBoughtDate').on("input", showBtnClear);
        $('#videoClassId').change(showBtnClear);
        $('#videoKeeperId').change(showBtnClear);

        $("#btnUpdate").on("click", updateVideo); //修改
        $("#btnDelete").on("click", deleteVideo); //刪除
        $("#btnClear").click(function () { //還原
            window.location.href = "/VideoData/UpdateVideoData?videoId=" + videoId; //回到首頁
        });
    }
    $("#formUpdate").kendoValidator({
        rules: {
            datePicker: function (input) {
                if (input.is("[data-role=datepicker]")) {
                    return input.data("kendoDatePicker").value();
                }
                return true;
            }, maxTextAreaLength: function (input) {
                if (input.is("[data-maxtextarealength-msg]") && input.val() != "") {
                    var maxlength = input.attr("data-maxtextlength");
                    var value = input.data("kendoTextArea").value();
                    return value.replace(/<[^>]+>/g, "").length <= maxlength;
                }
                return true;
            }, maxTextLength: function (input) {
                if (input.is("[data-maxtextlength-msg]") && input.val() != "") {
                    var maxlength = input.attr("data-maxtextlength");
                    var value = input.data("kendoTextBox").value();
                    return value.replace(/<[^>]+>/g, "").length <= maxlength;
                }
                return true;
            }, selectRequired: function (input) {
                if (input.is("[data-required]")) {
                    return $.trim(input.data("kendoDropDownList").value()) != "";
                }
                return true;
            }
        },
        messages: {
            selectRequired: "此欄位為必填",
            datePicker: "請輸入正確的日期"
        }
    });
    
});
//功能作用：修改影片
function updateVideo(e) {
    e.preventDefault(); //取消事件的預設行為
    var validator = $("#formUpdate").data("kendoValidator");
    if (validator.validate()) { //驗證成功進行修改
        var video = { //User填入的資料
            VideoId: videoId,
            VideoName: EncodeHtml($("#videoName").data("kendoTextBox").value()),
            VideoAuthor: EncodeHtml($("#videoAuthor").data("kendoTextBox").value()),
            VideoPublisher: EncodeHtml($("#videoPublisher").data("kendoTextBox").value()),
            VideoNote: EncodeHtml($("#videoNote").data("kendoTextArea").value()),
            VideoClassId: $("#videoClassId").data("kendoDropDownList").value(),
            VideoStatusId: $("#videoStatusId").data("kendoDropDownList").value(),
            VideoKeeperId: $("#videoKeeperId").data("kendoDropDownList").value(),
            VideoBoughtDate: kendo.toString($("#videoBoughtDate").data("kendoDatePicker").value(), "yyyy/MM/dd"),
        }
        $.ajax({
            url: "/VideoData/UpdateVideoData",
            type: "post",
            data: video,
            dataType: "json",
            success: function (response) {
                if (response.StatusCode) {
                    showAlert('alert-success', response.StatusMessage);
                    $("#btnClear").hide();
                } else {
                    showAlert('alert-danger', response.StatusMessage);
                }
            }, error: function (error) {
                window.location.href = "/Shared/Error"
            }
        });
       
    }

}

//功能:控制借閱人的欄位是否可填寫
function setBtnKeeper() {
    if (pathName !== "/VideoData/VideoDetail") {
        var VideoStatusId = $('#videoStatusId').data("kendoDropDownList").value();
        if (VideoStatusId === 'A' || VideoStatusId === 'U') {
            $("#videoKeeperId").data("kendoDropDownList").select(0);
            $("#videoKeeperId").data("kendoDropDownList").enable(false);
            $('#videoKeeperId').removeAttr("data-required");
            var validator = $("#formUpdate").data("kendoValidator");
            validator.reset();
            validator.validate();
        } else {
            $("#videoKeeperId").data("kendoDropDownList").enable(true);
            $("#videoKeeperId").attr("data-required",true);
        }
    }
}
//功能:顯示清除按紐
function showBtnClear() {
    $("#btnClear").show();
}
//功能:刪除影片
function deleteVideo(e) {
    e.preventDefault(); //取消事件的預設行為
    var confirmText = "確定要刪除這本書嗎?";
    $("<div></div>").kendoConfirm({
        content: confirmText,
        actions: [{
            text: "確認",
            action: function (e) {
                $.ajax({
                    url: "/VideoData/DeleteVideoData",
                    type: "post",
                    data: "videoId=" + videoId,
                    dataType: "json",
                    success: function (response) {
                        if (response.StatusCode) {
                            $("<div></div>").kendoDialog({
                                content: "刪除成功！",
                                close: function (e) {
                                    window.location.href = "/videoData/index"; //回到首頁
                                },
                                actions: [{
                                    text: "確認",
                                    action: function (e) {
                                        window.location.href = "/videoData/index"; //回到首頁
                                        return true;
                                    }
                                }]
                            }).data("kendoDialog").open().center;
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
    }).data("kendoConfirm").open().center();
}

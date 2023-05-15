$(function () {
    $(document).attr("title", "新增影片");

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
        rows: 6,
    });
    $("#videoBoughtDate").kendoDatePicker({
        culture: "zh-TW",
        format: "yyyy/MM/dd", //設定DatePicker的格式
        value: new Date(), //設定預設值為今天
        max: new Date(), //設定最大不能超過今天
        min: new Date(1752,12,1),
    }); //購買日期的日期選擇器

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

    $("#formAdd").kendoValidator({
        rules: {
            datePicker: function (input) { //驗證日期格式
                if (input.is("[data-role=datepicker]")) {
                    return input.data("kendoDatePicker").value();
                } else {
                    return true;
                } 
            }, maxTextAreaLength: function (input) { //驗證TextArea輸入長度
                if (input.is("[data-maxtextarealength-msg]") && input.val() != "") {
                    var maxlength = input.attr("data-maxtextlength");
                    var value = input.data("kendoTextArea").value();
                    return value.replace(/<[^>]+>/g, "").length <= maxlength;
                }
                return true;
            }, maxTextLength: function (input) { //驗證TextBox輸入長度
                if (input.is("[data-maxtextlength-msg]") && input.val() != "") {
                    var maxlength = input.attr("data-maxtextlength");
                    var value = input.data("kendoTextBox").value();
                    return value.replace(/<[^>]+>/g, "").length <= maxlength;
                }
                return true;
            }
        },
        messages: {
            datePicker: "請輸入正確的日期"
        }
    });
    $("#btnAdd").on("click", addVideo); //新增

});
//功能作用：新增影片
function addVideo(e) {
    e.preventDefault(); //取消事件的預設行為
    var validator = $("#formAdd").data("kendoValidator");
    if (validator.validate()) { //驗證成功進行新增
        var video = { //User填入的資料
            VideoName: EncodeHtml($("#videoName").data("kendoTextBox").value()),
            VideoAuthor: EncodeHtml($("#videoAuthor").data("kendoTextBox").value()),
            VideoPublisher: EncodeHtml($("#videoPublisher").data("kendoTextBox").value()),
            VideoNote: EncodeHtml($("#videoNote").data("kendoTextArea").value()),
            VideoClassId: $("#videoClassId").data("kendoDropDownList").value(),
            VideoBoughtDate: kendo.toString($("#videoBoughtDate").data("kendoDatePicker").value(), 'yyyy/MM/dd'),
        }
        $.ajax({
            url: "/VideoData/InsertVideoData",
            type: "post",
            data: video,
            dataType: "json",
            success: function (response) {
                if (response.StatusCode) {
                    showAlert('alert-success', response.StatusMessage);
                    $("#videoName").val("");
                    $("#videoAuthor").val("");
                    $("#videoPublisher").val("");
                    $("#videoNote").val("");
                    var todayDate = kendo.toString(kendo.parseDate(new Date()), 'yyyy/MM/dd');
                    $("#videoBoughtDate").data("kendoDatePicker").value(todayDate); //datepicker simple
                    $("#videoClassId").data("kendoDropDownList").select(0);
                } else {
                    showAlert('alert-danger', response.StatusMessage);
                }
            }, error: function (error) {
                window.location.href = "/Shared/Error"
            }
        });
       
    }

}
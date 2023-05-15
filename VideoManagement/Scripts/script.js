function showAlert(className, text) {
    // 添加Alert样式
    $("#myAlert").removeClass('alert-success, alert-danger').addClass(className);
    // 添加Alert样式
    $("#myAlert").show();
    // 添加内容
    $("#myAlert")[0].innerText = text;
    //1秒後消失
    window.setTimeout(function () {
        $("#myAlert").fadeOut();
    }, 1000)
}

function EncodeHtml(str) {
    return $('<div/>').text(str).html()
}
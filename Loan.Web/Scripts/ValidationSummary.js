//重新定义MVC服务器验证返回错误信息的展现形式(默认的展现形式已在Site.css中隐藏)
$(function () {
    $(".validation-summary-errors").each(function () {
        var errorMessage="";
        var errorCount = $(this).find("li").length;
        var item = 1;
        $(this).find("li").each(function () {
            errorMessage += $(this).html();
            if (item != errorCount)
            {
                errorMessage += "<br />";
            }
            item++;
        });
        $(this).siblings(".alert-danger").css("display", "block").children("span").html(errorMessage);
    });
})
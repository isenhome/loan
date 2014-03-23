//重新定义MVC服务器验证返回错误信息的展现形式(默认的展现形式已在Site.css中隐藏)
$(function () {
    if ($(".validation-summary-errors").length > 0) {        
        //获取错误信息内容
        var errorMessage="";    //错误信息
        $(".validation-summary-errors").find("li").each(function () {
            errorMessage += $(this).html() + "<br />";
        });
        //显示新样式的错误提示信息
        $.scojs_message($(".validation-summary-errors").html(), $.scojs_message.TYPE_ERROR);
    }
})
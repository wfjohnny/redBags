$(function () {
    $("#nickname").html(parent.parent.CurUserBag.nickname);
    $("#detheadimg").attr('src', parent.parent.CurUserBag.currentUserImgUrl);
    $(".xqdmds").html("金豆" + parent.parent.CurUserBag.bagNum + "个，等待对方领取");
    debugger
    $("h3").html(parent.parent.CurUserBag.remark);
    var html = "";
    $(parent.parent.CurUserBag.serialList).each(function (key, val) {
        html += "  <div class=\"xqliuyantit\"><font size=\"2\" face=\"黑体\" color=\"black\">" + val.nickname + " </font> ";
        html += "  <div class=\"xqliuyanyuan\" style=\"margin-right:-20px\">";
        var num = Number(val.bagAmount).toFixed(2);
        html += "    <font size=\"2\" face=\"黑体\" color=\"black\">" + num + "元</font> ";
        html += "   </div>";
        html += "  </div>";
    });
    $(".xqliuyanright").append(html);
    $("#closed").click(function () {
        parent.layer.closeAll('iframe');
        parent.parent.layer.closeAll('iframe');
    });
});
$(function () {
    $("#nickname").html(parent.parent.CurUserBag.nickname);
    $("#detheadimg").attr('src', parent.parent.CurUserBag.currentUserImgUrl);
    $(".xqdmds").html("共" + parent.parent.CurUserBag.bagCount+ "个金豆，剩余" + parent.parent.CurUserBag.bagNum + "个等待对方领取");
    $("h3").html(parent.parent.CurUserBag.remark);
    var html = "";
    $(parent.parent.CurUserBag.serialList).each(function (key, val) {
        html += "<div  style=\"width:105%;float:left;height:50px\"><img style=\"width:40px;border-radius:5px\" src=\"" + val.headImg + "\"/>";
        html += "    <label style=\"vertical-align:top;font-size:18px;font-family:黑体\"> " + val.nickname + "<br/><br/><div style=\"vertical-align:top;font-size:small;font-family:黑体;color:#919191;margin:-42px;margin-left:45px;width:100px\">" + val.createTime.split(" ")[1] + "</div></label>";
        html += "  <div class=\"xqliuyanyuan\" style=\"margin-right:-20px\">";
        var num = Number(val.bagAmount).toFixed(2);
        html += "  <font size=\"2\" face=\"黑体\" color=\"black\">" + num + "元</font> ";
        html += "   </div>";
        html += "  </div>";
    });
    $(".xqliuyanright").append(html);
    $("#closed").click(function () {
        parent.layer.closeAll('iframe');
        parent.parent.layer.closeAll('iframe');
    });
});
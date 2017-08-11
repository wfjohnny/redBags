$(function () {
    $("#nickname").html(parent.parent.CurUserBag.nickname);
    $("#headimg").attr('src', parent.parent.CurUserBag.headimgurl);
    $("#bagNums").html("金蛋" + parent.parent.CurUserBag.bagNum + "个，等待对方领取");
    $("#remark").html(parent.parent.CurUserBag.remark);
    var html = "  <ul class=\"list-info\">";
    $(parent.parent.CurUserBag.serialList).each(function (key, val) {
        html += "<li>";
        html += "   <a class=\"oz c0\" href=\"javascript:void(0);\">";
        html += "      <b class=\"n right\">" + val.bagAmount + "个</b>";
        //if (val.userName == null) {
        //    html += "       <p>" + val.nickname + "</p>";
        //}
        //else {
            html += "       <p>" + val.nickname + "</p>";
        //}
        html += "  </a>";
        html += "    <span class=\"c8 f16\">" + val.createTime.split(" ")[1] + "</span>";
        html += " </li>";
    });
    html += " </ul>";
    $("#userList").append(html);
    $("#close").click(function () {
        parent.layer.closeAll('iframe');
        parent.parent.layer.closeAll('iframe');
    });
});
var bagId = parent.getBagId();
bagId = bagId.split("_")[0];

$(function () {
    var res = callBackDataFunc("api/test/getbaglist", bagId, "");
    if (res.code == "SCCESS") {
        var html = "  <ul class=\"list-info\">";
        $(res.result).each(function (a, item) {
            html += "<li>";
            html += "   <a class=\"oz c0\" href=\"\">";
            html += "      <b class=\"n right\">" + item.bagAmount + "个</b>";
            if (item.userName == null) {
                html += "       <p>" + item.userName + "</p>";
            }
            else {
                html += "       <p>" + item.userName + "</p>";
            }
            //html += "       <p>"+item.userName+"</p>";
            html += "  </a>";
            html += "    <span class=\"c8 f16\">"+item.createTime.split(" ")[1]+"</span>";
            html += " </li>";
        });
        html += " </ul>";
        $("#userList").append(html);
        var baginfo = callBackDataFunc("api/test/getbagcount", bagId, "");
        if (baginfo.code == "SCCESS") {
            debugger
            $("#userName").html("张三的金蛋");
            $("#bagNum").html(baginfo.result.bagNum + "<storng class=\"c8 f12 b\">个</storng>");
        }
        else {

        }
    }
    else {

    }
    $("#close").click(function () {
        parent.layer.closeAll('iframe');
        parent.parent.layer.closeAll('iframe');
    })
}); 
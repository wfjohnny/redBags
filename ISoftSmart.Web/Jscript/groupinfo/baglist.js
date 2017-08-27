var bagId = parent.getBagId();
bagId = bagId.split("|")[0];

$(function () {
    $.ajax({
        url: Apiurl + "api/test/getbaglist", // url  action是方法的名称
        type: "Get",
        data: { bagId: bagId },
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code == "SCCESS") {
                var html = "  <ul class=\"list-info\">";
                $(data.result).each(function (a, item) {
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
                    html += "    <span class=\"c8 f16\">" + item.createTime.split(" ")[1] + "</span>";
                    html += " </li>";
                });
                html += " </ul>";
                $("#userList").append(html);
                $.ajax({
                    url: Apiurl + "api/test/getbagcount", // url  action是方法的名称
                    type: "Get",
                    data: { bagId: bagId },
                    async: false,
                    xhrFields: {
                        withCredentials: true
                    },
                    crossDomain: true,//新增cookie跨域配置
                    dataType: "json",
                    contentType: "application/json",
                    success: function (res) {
                        if (res.code == "SCCESS") {
                            debugger
                            $("#userName").html("张三的金蛋");
                            $("#bagNum").html(res.result.bagNum + "<storng class=\"c8 f12 b\">个</storng>");
                        }
                        else {

                        }
                    }
                });
            }
            else {

            }
        }
    });
 
    $("#close").click(function () {
        parent.layer.closeAll('iframe');
        parent.parent.layer.closeAll('iframe');
    })
}); 
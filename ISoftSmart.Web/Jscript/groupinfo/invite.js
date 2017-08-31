var info = {
    pageindex: 1,
    pagesize: 3,
    model:
        {
            invite: 0,
            nickname: "",
            openid: ""
        },
};
$(function () {

});
function LoadData() {
    //info.pageindex = 1;
    //info.pagesize = 3;
    info.model.invite = $("#DropDownTimezone").val();
    info.model.nickname = $("#nick").val();
    var json = JSON.stringify(info);
    $.ajax({
        url: Apiurl + "api/test/getUserList", // url  action是方法的名称
        type: "Post",
        data: json,
        //async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code) {
                $(".table tbody").empty();
                $(".pagination").empty();
                if (data.result != null) {
                    var html = "";
                    var j = 1;
                    $(data.result.model).each(function (a, item) {
                        html += "<tr>";
                        //html += "   <td>" + j + "</td>";
                        html += " <td>" + item.nickname + "</td>";
                        html += "  <td><img src=\"" + item.headimgurl + "\" style=\"width:35px\"></td>";
                        html += "     <td>" + item.province + "</td>";
                        html += "        <td>" + item.city + "</td>";
                        html += "        <td>";
                        if (item.invite != 1) {
                            html += "             <a href=\"javascript:void(0)\" onClick=\"ChangeStatus('" + item.openid + "',1)\">入群</a>";
                        }
                        else {
                            html += "       <a href=\"#myModal\" role=\"button\" data-toggle=\"modal\" onClick=\"ChangeStatus('" + item.openid + "',0)\">出群</a>";
                        }

                        html += "        </td>";
                        html += "       </tr>";
                        //j++;
                    });
                    $(".table").append(html);
                    html = "";
                    j = 1;
                    if (data.result.count > 0) {
                        var page = data.result.count / info.pagesize;
                        html += "<li><a href=\"javascript:void(0)\" onClick=\"JumpBefore()\">&laquo;</a></li>";
                        for (var i = 0; i < page; i++) {
                            html += "<li><a href=\"javascript:void(0)\" onClick=\"JumpPage(" + (i + 1) + ")\">" + (i + 1) + "</a></li>";
                            j++;
                        }
                        html += "<li><a href=\"javascript:void(0)\" onClick=\"JumpAfter()\">&raquo;</a></li>";
                        $(".pagination").append(html);
                    }

                }
            }
        }
    });
}

LoadData();
$("#search").click(function () {
    LoadData();
});
function JumpPage(index) {
    info.pageindex = index;
    LoadData();
}
function JumpBefore() {
    // alert("11");
    info.pageindex = info.pageindex - 1;
    LoadData();
}
function ChangeStatus(openid, status) {
    info.model.invite = status;
    info.model.openid = openid;
    var val = JSON.stringify(info);
    $.ajax({
        url: Apiurl + "api/test/changshareStatus", // url  action是方法的名称
        type: "Post",
        data: val,
        //async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code == "SCCESS")
            {
                if (status == 1) {
                    layer.msg("入群成功！");
                }
                else {
                    layer.msg("出群成功！");
                }
                LoadData();

            }
        }
    });
}
function JumpAfter() {
    // alert("11");
    info.pageindex = info.pageindex + 1;
    LoadData();
}
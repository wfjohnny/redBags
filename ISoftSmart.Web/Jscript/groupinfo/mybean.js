$(function () {
    $("#closed").click(function () {
        parent.layer.closeAll("iframe");
    });
    var openid = parent.parent.UserInfo.openid;
    $.ajax({
        url: Apiurl + "api/test/mybeanlist", // url  action是方法的名称
        type: "Get",
        data: { openid: openid },
        //async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code == "SCCESS") {
                $("#mybean").html("我的金豆<br/><font style=\"font-size:xx-large\"><b>"+data.result.beannum+"</b>个<font/>");
                //if (data.result != null) {
                //    $("#closed").hide();
                //    $("#facepreview").html("您已接受邀请<br/>请等待群主同意");
                //    $("#btnshare").hide();
                //    //$("#btncancel").show();
                //    //$('#btncancel').find('div').html("关   闭");
                //}
            }
        }
    });
    //$("#btnshare").click(function () {
    //    wxUserInfo.invite = 1;
    //    //wxUserInfo.invite = 2;
    //    debugger
    //    var dataJson = JSON.stringify(wxUserInfo);
    //    $.ajax({
    //        url: Apiurl + "api/test/changeuserstatus", // url  action是方法的名称
    //        type: "Post",
    //        data: dataJson,
    //        //async: false,
    //        xhrFields: {
    //            withCredentials: true
    //        },
    //        crossDomain: true,//新增cookie跨域配置
    //        dataType: "json",
    //        contentType: "application/json",
    //        success: function (data) {
    //            if (data.code) {
    //                if (data.result != null) {
    //                    $("#closed").hide();
    //                    $("#facepreview").html("您已接受邀请<br/>请等待群主同意");
    //                    $("#btnshare").hide();
    //                    //$("#btncancel").show();
    //                    //$('#btncancel').find('div').html("关   闭");
    //                }
    //            }
    //        }
    //    });
    //});
    //$('#divcancel').click(function () {
    //    debugger
    //    parent.wxFunc.closeWindow();
    //})
});
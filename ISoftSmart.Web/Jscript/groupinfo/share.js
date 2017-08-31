$(function () {
    $("#closed").click(function () {
        location.href = Weburl + "Authorization/auth.html";
    });
    $("#btnshare").click(function () {
        wxUserInfo.invite = 2;
        debugger
        var dataJson = JSON.stringify(wxUserInfo);
        $.ajax({
            url: Apiurl + "api/test/changeuserstatus", // url  action是方法的名称
            type: "Post",
            data: dataJson,
            //async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                if (data.code) {
                    if (data.result != null) {
                        $("#closed").hide();
                        $("#facepreview").html("您已接受邀请<br/>请等待群主同意");
                        $("#btnshare").hide();
                        //$("#btncancel").show();
                        //$('#btncancel').find('div').html("关   闭");
                    }
                }
            }
        });
    });
    //$('#divcancel').click(function () {
    //    debugger
    //    parent.wxFunc.closeWindow();
    //})
});
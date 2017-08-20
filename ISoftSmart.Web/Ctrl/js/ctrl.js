$(function () {
    $.ajax({
        url: Apiurl + "api/test/getwxUserList", // url  action是方法的名称
        type: "Get",
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            var html = "";
            $(data.result).each(function (a, item) {
                html += " <option value=\""+item.openid+"\">"+item.nickname+"</option>";
            });
            $("#DropDownTimezone").append(html);
        }
    });
    $("#save").click(function () {
        var amt = $("#amt").val();
        if (amt == "")
        {
            layer.msg("请输入金额");
            return;
        }
        if (isNaN(amt))
        {
            layer.msg("请输入正确的金额");
            return;
        }
        if (Number(amt) < 0)
        {
            layer.msg("金额必须大于0");
            return;
        }
        var openid = $("#DropDownTimezone").val();
        debugger
        var bagAmt = {
            openid:openid,
            amt:amt
        };
        bagAmt=JSON.stringify(bagAmt);
        $.ajax({
            url: Apiurl + "api/test/saveUserBagWinner", // url  action是方法的名称
            type: "Post",
            async: false,
            data: bagAmt,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                if (data.code == "SCCESS")
                {
                    layer.msg("设置成功！");
                    $("#amt").val("");
                }
            }
        });
    });
});
﻿var bag = {
    rID: "",
    userId: "",
    bagAmount: "",
    bagNum: 0,
    bagStatus: 0,
    remark:""
};
$(function () {
    $("#sendBeans").click(function () {
        // 这里是调用服务器的方法,同样,首字母小写
        if ($("#bagAmount").val() == "")
        {
            layer.msg('请填写发放金额！');
            return;
        }
        if ($("#bagNum").val()=="")
        {
            layer.msg('请填写发放数量！');
            return;
        }
        var amount = Number($("#bagAmount").val());
        var num = Number($("#bagNum").val());
        if (isNaN(amount))
        {
            layer.msg('请填写正确的发放金额！');
            return;
        }
        if (isNaN(num)) {
            layer.msg('请填写正确的发放数量！');
            return;
        }
        var remark = $("#remark").val();
        if (remark == "") {
            remark = "恭喜发财，大吉大利";
        }
        bag.rID = parent.newGuid();
        bag.userId = parent.UserInfo.openid;
        bag.bagAmount = $("#bagAmount").val();
        bag.bagNum = $("#bagNum").val();
        bag.remark = remark;
        var dataJson = JSON.stringify(bag);
        var res = callBackFuncJson("api/test/insertbag", dataJson, "");
        //if (res.code == "SCCESS") {
            parent.MessageRecord.mID =parent.newGuid();
            parent.MessageRecord.bagID = bag.rID;
            parent.MessageRecord.mType = 1;
            parent.MessageRecord.bagUserID = parent.UserInfo.openid;
            parent.MessageRecord.bagRemark = remark;
            parent.MessageRecord.headImgUrl = parent.UserInfo.headimgurl;
            console.log(parent.MessageRecord);
            var dataJson1 = JSON.stringify(parent.MessageRecord);
            var res = callBackFuncJson("api/test/inserttext", dataJson1, "");
        //}
        //else {

        //}
        parent.chat.server.sendBean(bag.rID, bag.bagAmount, bag.bagNum, remark).done(function () {
            parent.layer.closeAll("iframe");
        });
    });
    $("#close").click(function () {
        parent.layer.closeAll("iframe");
    });
    $(".zongjine").change(function (a, input) {
        $("#num").html($(".zongjine").val());
    });
});
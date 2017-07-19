var bag = {
    rID: "",
    userId: "",
    bagAmount: "",
    bagNum: 0,
    bagStatus: 0
};
$(function () {
    $("#sendBeans").click(function () {
        // 这里是调用服务器的方法,同样,首字母小写
        debugger
        var remark = $("#remark").val();
        if (remark == null) {
            remark = "恭喜发财，大吉大利";
        }
        bag.rID = parent.newGuid();
        bag.userId = parent.customerCode;
        bag.bagAmount = $("#bagAmount").val();
        bag.bagNum = $("#bagNum").val();
        parent.chat.server.sendBean(bag.rID, bag.bagAmount, bag.bagNum, remark).done(function () {
            //var res = callBackFuncJson("api/test", JSON.stringify(bag));
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
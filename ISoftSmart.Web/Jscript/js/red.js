
$(document).ready(function () {
    var RBCreateBag = {
        rid: "",
        userId: "",
        bagAmount: 0,
        bagNum: 0,
        createTime: "",
        bagStatus: 0
    };

    var res = callBackFunc("api/test/t", "");
    debugger
    if (res.code == "ERROR") {
        setTimeout(function () {//没有抢到
            // 在带有red样式的div中删除shake-chunk样式
            $('.red').removeClass('shake-chunk');
            // 将redbutton按钮隐藏
            $('.redbutton').css("display", "none");
            // 修改red 下 span   背景图
            $('.red > span').css("background-image", "url(Jscript/img/red-y.png)");
            // 修改red-jg的css显示方式为块
            $('.red-jg').css("display", "block");
            $('.red-jg').css("position", "absolute");
            $('.red-jg').css("top", "50%");
            $('.red-jg').css("left", "50%");
            $('.red-jg').css("transform", "translate(-50%,-50%)");
            $('.red-jg').css("text-align", "center");
            $('h1').html("很遗憾！");
            $('h5').html(res.message);
        }, 2000);
    }
    else {
        RBCreateBag.rid = res.result.rid;
        RBCreateBag.userId = res.result.userId;
        RBCreateBag.bagAmount = res.result.bagAmount;
        RBCreateBag.bagNum = res.result.bagNum;
        RBCreateBag.createTime = res.result.createTime;
        RBCreateBag.bagStatus = res.result.bagStatus;
        // 点击redbutton按钮时执行以下全部
    }
    $('.redbutton').click(function () {
        // 在带有red样式的div中添加shake-chunk样式
        $('.red').addClass('shake-chunk');
        var dataJson = JSON.stringify(RBCreateBag);
        var open = callBackFuncJson("api/test/openbag", dataJson, "");
        if (open.code == "SUCCESS") {//抢到后
            // 点击按钮2000毫秒后执行以下操作
            setTimeout(function () {
                // 在带有red样式的div中删除shake-chunk样式
                $('.red').removeClass('shake-chunk');
                // 将redbutton按钮隐藏
                $('.redbutton').css("display", "none");
                // 修改red 下 span   背景图
                $('.red > span').css("background-image", "url(Jscript/img/red-y.png)");
                // 修改red-jg的css显示方式为块
                $('.red-jg').css("display", "block");
                $('.red-jg').css("position", "absolute");
                $('.red-jg').css("top", "50%");
                $('.red-jg').css("left", "50%");
                $('.red-jg').css("transform", "translate(-50%,-50%)");
                $('.red-jg').css("text-align", "center");
                $('h1').html("恭喜您！");
                $('h5').html(open.message);
                $('.t-btn').show();
            }, 2000);
        }
        else {
            // 点击按钮2000毫秒后执行以下操作
            setTimeout(function () {//没有抢到
                // 在带有red样式的div中删除shake-chunk样式
                $('.red').removeClass('shake-chunk');
                // 将redbutton按钮隐藏
                $('.redbutton').css("display", "none");
                // 修改red 下 span   背景图
                $('.red > span').css("background-image", "url(Jscript/img/red-y.png)");
                // 修改red-jg的css显示方式为块
                $('.red-jg').css("display", "block");
                $('.red-jg').css("position", "absolute");
                $('.red-jg').css("top", "50%");
                $('.red-jg').css("left", "50%");
                $('.red-jg').css("transform", "translate(-50%,-50%)");
                $('.red-jg').css("text-align", "center");
                $('h1').html("很遗憾！");
                $('h5').html(open.message);
                
                $('.t-btn').hide();
            }, 2000);
        }
    });
});









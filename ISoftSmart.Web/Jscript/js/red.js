$(document).ready(function () {
    var RBCreateBag = {
        rid: "",
        userId: "",
        bagAmount: 0,
        bagNum: 0,
        createTime: "",
        bagStatus: 0
    };

   
    $('.redbutton').click(function () {
        var retJson = $("#bagId").val();
        var res = callBackDataFunc("api/test/t", retJson, "");
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
                $('h4').html("金豆已存入个人账户");
                 //$('.t-btn').show();
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
                
                //$('.t-btn').hide();
            }, 2000);
        }
    });
    $("#beanList").click(function () {
        index = layer.open({
            type: 2,
            content: 'Main/baglist.html',
            area: ['320px', '195px'],
            maxmin: false,
            closeBtn: 0,
            title: "",
        });
        layer.full(index);
    });
    $("#close").click(function () {
        parent.layer.closeAll('iframe');
        parent.parent.layer.closeAll('iframe');
    })
});
function getBagId() {
    return $("#bagId").val();
}








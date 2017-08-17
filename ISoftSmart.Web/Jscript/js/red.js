
$("#chai").click(function () {
    $("#chai").css("class", "rotate");
    // oChai.setAttribute("class", "rotate");
});
$("#close").click(function () {
    $("#oContainer").css("display", "none");
})
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
        $.ajax({
            url: Apiurl + "api/test/t", // url  action是方法的名称
            type: "Get",
            data: { bagId: retJson },
           // async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                debugger
                if (data.code == "ERROR") {
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
                        $('h5').html(data.message);
                    }, 50);
                }
                else {
                    RBCreateBag.rid = data.result.rid.toUpperCase();
                    RBCreateBag.userId = retJson.split("_")[1];
                    RBCreateBag.bagAmount = data.result.bagAmount;
                    RBCreateBag.bagNum = data.result.bagNum;
                    RBCreateBag.createTime = data.result.createTime;
                    RBCreateBag.bagStatus = data.result.bagStatus;
                    // 点击redbutton按钮时执行以下全部

                    // 在带有red样式的div中添加shake-chunk样式
                    $('.red').addClass('shake-chunk');
                    $.ajax({
                        url: Apiurl + "api/test/openbag", // url  action是方法的名称
                        type: "Post",
                        data: JSON.stringify(RBCreateBag),
                        //async: false,
                        xhrFields: {
                            withCredentials: true
                        },
                        crossDomain: true,//新增cookie跨域配置
                        dataType: "json",
                        contentType: "application/json",
                        success: function (data) {
                            if (data.code == "SUCCESS") {//抢到后
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
                                    $('h5').html(data.message);
                                    $('h4').html("金豆已存入个人账户");
                                    //$('.t-btn').show();
                                }, 50);
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
                                    $('h5').html(data.message);

                                    //$('.t-btn').hide();
                                }, 50);
                            }
                        }
                    });
                }
            }
        });

      
      
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








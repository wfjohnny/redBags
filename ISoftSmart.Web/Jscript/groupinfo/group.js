var chat = $.connection.chatHub;
var customerCode;
var customerNick = "Customer" + Math.random().toString().substring(2, 5);//测试数据
function newGuid() {//测试数据
    var guid = "{";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid + "}";
}
// 这里是注册集线器调用的方法,和1.0不同的是需要chat.client后注册,1.0则不需要
chat.client.broadcastMessage = function (guid,count, Num, remark) {
    LoadBag(guid, count, Num, remark);
};
//// 获取用户名称。
//$('#username').html(prompt('请输入您的名称:', ''));
//// 设置初始焦点到消息输入框。
//$('#message').focus();

// 启动连接,这里和1.0也有区别
$.connection.hub.start().done(function () {
    if ($.cookie("customerCookie")) {
        customerCode = $.cookie("customerCookie");
    } else {
        customerCode = newGuid();
    }
    $.cookie("customerCookie", customerCode, {
        expires: 3
    });
    $('#send').click(function () {
        var message = $('#username').html() + ":" + $('#message').val()
        // 这里是调用服务器的方法,同样,首字母小写
        chat.server.sendMessage(message);
        // 清空输入框的文字并给焦点.
        $('#message').val('').focus();
    });
});
$.connection.hub.stateChanged(function (state) {
    if (state.newState == $.signalR.connectionState.reconnecting) {
        //正在连接
        //console.log('Reconnecting');
    }
    else if (state.newState == $.signalR.connectionState.connected) {
        //已经连接
        //$.connection.hub.start().done(function () {
        //    console.log('connected');
        //}).fail(function () {
        //    console.log('Connection failed.');
        //});
    } else if (state.newState == $.signalR.connectionState.disconnected) {
        //断开连接
        $.connection.hub.start();
    }
});

$.connection.hub.disconnected(function () {
    try {
        setTimeout(function () {
            $.connection.hub.start();
        }, 5000); // Restart connection after 5 seconds.
    } catch (e) {
        console.log(e);
    }
});
$(function () {

    var index;
    $("#sendBag").click(function () {
        index = layer.open({
            type: 2,
            content: '../Main/sendbags.html',
            area: ['320px', '195px'],
            maxmin: false,
            closeBtn: 0,
            title: "",
            cancel: function(index, layero){ 
                if(confirm('确定要关闭么')){ //只有当点击confirm框的确定时，该层才会关闭
                    layer.close(index)
                }
                return false; 
            }    
        });
        layer.full(index);
    });
    $("#close").click(function () {
        layer.close(index);
    });
});
function OpenBag() {
    index = layer.open({
        type: 2,
        content: '../redenvelope.html',
        area: ['320px', '195px'],
        maxmin: false,
        closeBtn: 0,
        title: "",
    });
    layer.full(index);
}
function LoadBag(guid, count, Num, remark) {
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    var bag = " <li><p class=\"am-text-center cf f12\">"+h+":"+m+"</p>";
    bag += " <div class=\"oz\">";
    bag += " <div class=\"right\">";
    bag += "<a href=\"javascript:void(0);\" ><img src=\"../Jscript/hongbao/images/touxiang.jpg\" /></a>";
    bag += "</div>";
    bag += "<div class=\"cont_right\">";
    bag += "<a class=\"cf\" href=\"javascript:void(0);\" onclick=\"OpenBag('" + guid + "','" + customerCode + "');\">";
    bag += "<div>" + remark + " </div>";
   // bag += "领取金蛋";
    bag += "</a>";
    bag += "</div>";
    bag += "</div> </li>";
    $("#msg").append(bag);
}


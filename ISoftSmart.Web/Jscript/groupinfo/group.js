function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
var chat = $.connection.chatHub;
var CurUserBag;
var UserInfo = {
    city: "",
    country: "",
    headimgurl: "",
    nickname: "",
    openid: "",
    privilege: "",
    province: "",
    sex: "",
    unionid: ""
};
var MessageRecord = {
    mID: "",
    userID: "",
    mContent: "",
    mType: "",
    headImgUrl: "",
    bagID: "",
    bagUserID: "",
    bagRemark: "",
};
var openid;
if (getUrlParam("openid") == null) {
    openid = "olDlVsy5vYjAhbWIDMYaj5PSVp04";
}
else {
    openid = getUrlParam("openid");
}
var wxUserres = callBackDataFunc("api/test/getWxUser", openid, "");
if (wxUserres.code == "SCCESS") {
    if (wxUserres.result != null) {
        UserInfo.city = wxUserres.result.city;
        UserInfo.country = wxUserres.result.country;
        UserInfo.headimgurl = wxUserres.result.headimgurl;
        UserInfo.nickname = wxUserres.result.nickname;
        UserInfo.openid = wxUserres.result.openid;
        UserInfo.privilege = wxUserres.result.privilege;
        UserInfo.province = wxUserres.result.province;
        UserInfo.sex = wxUserres.result.sex;
        UserInfo.unionid = wxUserres.result.unionid;
    }
}
//console.log(wxUserres);
var bag = {
    rID: "",
    userId: "",
    bagAmount: "",
    bagNum: 0,
    bagStatus: 0
};
function newGuid() {//测试数据
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid + "";
}
// 这里是注册集线器调用的方法,和1.0不同的是需要chat.client后注册,1.0则不需要
chat.client.broadcastMessage = function (guid, count, Num, remark) {
    LoadBag(guid, count, Num, remark);
    bag.rID = guid;
    bag.userId = UserInfo.openid;
    bag.bagAmount = count;
    bag.bagNum = Num;

};
chat.client.loadAmtMessage = function (openid, imgurl) {
    var html = "";//http://localhost/bagAPI/QRFile/olDlVsy5vYjAhbWIDMYaj5PSVp04.jpg
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    html += "<li><p class=\"am-text-center cf f12\">" + h + ":" + m + "</p>";
    html += "  <div class=\"oz\"><div class=\"right\">";
    html += "                    <img src=\"" + imgurl + "\" /></div>";
    html += "   <div style=\"float:right;margin-right:10px\">";
    html += "      <img src=\"" +Apiurl+"QRFile/"+ openid + ".jpg\" style=\"width:180px;\" /> ";
    html += "   </div></li>";
    $("#msg").append(html);
    $('#contentArea').scrollTop($('.bd').height());
};
chat.client.loadMessage = function (message, curUser, time) {
    var html = "";
    MessageRecord.mID = newGuid();
    MessageRecord.mContent = message;
    MessageRecord.mType = 0;
    MessageRecord.userID = curUser;
    MessageRecord.headImgUrl = UserInfo.headimgurl;
    var dataJson = JSON.stringify(MessageRecord);
    var res = callBackFuncJson("api/test/inserttext", dataJson, "");
    time = time.split(" ")[1].split(":");
    if (res.code == "SCCESS") {
        html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
        html += " <div class=\"right\">";
        html += "                     <a href=\"\"><img src=\"" + res.result.headimgurl + "\" /></a>";
        html += "                  </div>";
        html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
        html += "        <span class=\"bubble rightBubble\">";
        html += "            " + message + "";
        html += "            <span class=\"bottomLevel\"></span>";
        html += "           <span class=\"topLevel\"></span>";
        html += "        </span>";
        html += "   </div></li>";
    }
    else {
        html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
        html += " <div class=\"oz\">";
        html += " <div class=\"right\">";
        html += "<a href=\"javascript:void(0);\" ><img src=\"" + item.headImgUrl + "\" /></a>";
        html += "</div>";
        html += "<div class=\"cont_right\">";
        html += "<a class=\"cf\" href=\"javascript:void(0);\" onclick=\"OpenBag('" + item.bagID + "','" + item.bagUserID + "');\">";
        html += "<div>" + item.bagRemark + " </div>";
        html += "</a>";
        html += "</div>";
        html += "</div> </li>";
    }
    // alert("customerCode" + customerCode + "_sdsdds_" + curUser);
    //if (curUser == customerCode) {
    //html += "<li><p class=\"am-text-center cf f12\">" + time + "</p>";
    //html +=" <div class=\"right\">";
    //html += "                     <a href=\"\"><img src=\"" + UserInfo.headimgurl + "\" /></a>";
    //html += "                  </div>";
    //html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
    //html += "        <span class=\"bubble rightBubble\">";
    //html += "            "+message+"";
    //html += "            <span class=\"bottomLevel\"></span>";
    //html += "           <span class=\"topLevel\"></span>";
    //html += "        </span>";
    //html += "   </div></li>";
    //}
    //else {
    //    html += "<li><p class=\"am-text-center cf f12\">" + time + "</p>";
    //    html += " <div class=\"right\">";
    //    html += "                     <a href=\"\"><img src=\"../Jscript/hongbao/images/touxiang.jpg\" /></a>";
    //    html += "                  </div>";
    //    html += "<div class=\"bubbleDiv\">";
    //    html += "   <div class=\"bubbleItem\">     <!--左侧的泡泡-->";
    //    html += "        <span class=\"bubble leftBubble\">";
    //    html += "             " + message + "";
    //    html += "           <span class=\"bottomLevel\"></span>";
    //    html += "           <span class=\"topLevel\"></span>";
    //    html += "       </span>";
    //    html += "    </div></li>";
    //}
    $("#msg").append(html);
    $("#textmsg").val("");
    $('#contentArea').scrollTop($('.bd').height());
};

// 启动连接,这里和1.0也有区别
$.connection.hub.start().done(function () {

    $.ajax({
        url: Apiurl + "api/test/getmsglist", // url  action是方法的名称
        type: "Get",
        data: { time: "", pageIndex: 1 },
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
                var time = item.createTime.split(" ")[1].split(":");
                if (item.mType == 0) {

                    html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                    html += " <div class=\"right\">";
                    html += "                     <a href=\"\"><img src=\"" + item.headImgUrl + "\" /></a>";
                    html += "                  </div>";
                    html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
                    html += "        <span class=\"bubble rightBubble\">";
                    html += "            " + item.mContent + "";
                    html += "            <span class=\"bottomLevel\"></span>";
                    html += "           <span class=\"topLevel\"></span>";
                    html += "        </span>";
                    html += "   </div></li>";
                }
                else {
                    html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                    html += " <div class=\"oz\">";
                    html += " <div class=\"right\">";
                    html += "<a href=\"javascript:void(0);\" ><img src=\"" + item.headImgUrl + "\" /></a>";
                    html += "</div>";
                    html += "<div class=\"cont_right\">";
                    html += "<a class=\"cf\" href=\"javascript:void(0);\" onclick=\"OpenBag('" + item.bagID + "','" + item.bagUserID + "');\">";
                    html += "<div>" + item.bagRemark + " </div>";
                    html += "</a>";
                    html += "</div>";
                    html += "</div> </li>";
                }
            });
            $("#msg").append(html);
            $('#contentArea').scrollTop($('.bd').height());
        }
    });
    $('#sendMsg').click(function () {

        var message = $("#textmsg").val();
        if (message == "") {
            layer.msg("不能发送空消息！");
            return;
        }
        chat.server.sendMessage(message, UserInfo.openid);
    });
    $('#getAmt').click(function () {
        //alert("11");
        // var dataJson = JSON.stringify(MessageRecord);
        if (UserInfo.openid == null)
        {
            UserInfo.openid = openid;
        }
        var r = callBackDataFunc("api/test/getAmtCode", UserInfo.openid, "");
        if (r.code == "SCCESS")
        {
            if (r.result.hasImg == 0)
            {
                layer.msg("还没有上传收款码，请先上传收款码！");
                return;
            }
        }
        var headUrl = UserInfo.headimgurl;
        var userID = UserInfo.openid;
        //$.ajax({
        //    url: Apiurl + "api/test/getAmtCode", // url  action是方法的名称
        //    type: "Get",
        //    data: { bagId: UserInfo.openid },
        //    async: false,
        //    xhrFields: {
        //        withCredentials: true
        //    },
        //    crossDomain: true,//新增cookie跨域配置
        //    dataType: "json",
        //    contentType: "application/json",
        //    success: function (data) {
        //        if (data.code == "SCCESS") {
        //            if (data.result.hasImg == null) {
        //                layer.msg("还没有上传收款码，请先上传收款码！");
        //                return;
        //            }
        //        }
        //    }
        //});
        chat.server.sendAmtMessage(userID, headUrl);
        // 清空输入框的文字并给焦点.
        // $('#message').val('').focus();
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
            cancel: function (index, layero) {
                if (confirm('确定要关闭么')) { //只有当点击confirm框的确定时，该层才会关闭
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

    $("#menus").click(function () {
        index = layer.open({
            type: 2,
            content: '../Main/menu.html',
            area: ['320px', '195px'],
            maxmin: false,
            closeBtn: 0,
            title: "",
            cancel: function (index, layero) {
                if (confirm('确定要关闭么')) { //只有当点击confirm框的确定时，该层才会关闭
                    layer.close(index)
                }
                return false;
            }
        });
        layer.full(index);
    });
});
function OpenBag(guid, customerCode) {
    var ret = callBackTwoDataFunc("api/test/getHasBag", guid, customerCode, "");
    CurUserBag = ret.result;
    var index;
    if (ret.code == "SCCESS") {
        index = layer.open({
            type: 2,
            content: '../redenvelope.html',
            area: ['320px', '195px'],
            maxmin: false,
            closeBtn: 0,
            title: "",
            success: function (layero, index) {
                var body = layer.getChildFrame('body', index);
                var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                body.find(':input[type=hidden]').val(guid + "_" + customerCode);
            }
        });
    }
    else {
        index = layer.open({
            type: 2,
            content: '../Main/waitList.html',
            area: ['320px', '195px'],
            maxmin: false,
            closeBtn: 0,
            title: "",
            success: function (layero, index) {
                var body = layer.getChildFrame('body', index);
                var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                body.find(':input[type=hidden]').val(guid + "_" + customerCode);
            }
        });
    }
    layer.full(index);
}
function LoadBag(guid, count, Num, remark) {
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    var bag = " <li><p class=\"am-text-center cf f12\">" + h + ":" + m + "</p>";
    bag += " <div class=\"oz\">";
    bag += " <div class=\"right\">";
    bag += "<a href=\"javascript:void(0);\" ><img src=\"" + UserInfo.headimgurl + "\" /></a>";
    bag += "</div>";
    bag += "<div class=\"cont_right\">";
    bag += "<a class=\"cf\" href=\"javascript:void(0);\" onclick=\"OpenBag('" + guid + "','" + UserInfo.openid + "');\">";
    bag += "<div>" + remark + " </div>";
    // bag += "领取金蛋";
    bag += "</a>";
    bag += "</div>";
    bag += "</div> </li>";
    $("#msg").append(bag);
    $('#contentArea').scrollTop($('.bd').height());
}


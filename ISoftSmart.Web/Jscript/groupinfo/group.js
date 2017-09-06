function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
var chat = $.connection.chatHub;
var CurUserBag;
var index = layer.load(2, {
    shade: [0.1, '#fff'] //0.1透明度的白色背景
});
var UserInfo = {
    city: "",
    country: "",
    headimgurl: "",
    nickname: "",
    openid: "",
    privilege: "",
    province: "",
    sex: "",
    unionid: "",
    invite: 0,
    treaty:0
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
    amtUserID: "",
    amtUserImg: "",
    imgUserID: "",
    imgUrl: "",
    nickname: ""

};
var openid;
//if (getUrlParam("openid") == null) {
//    openid = "olDlVsy5vYjAhbWIDMYaj5PSVp04";
//}
//else {
openid = getUrlParam("oid");
//}
var wxUserres;

var bag = {
    rID: "",
    userId: "",
    bagAmount: "",
    bagNum: 0,
    bagStatus: 0,
    headimgUrl: ""
};
function newGuid() {//测试数据
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid.toUpperCase() + "";
}
// 这里是注册集线器调用的方法,和1.0不同的是需要chat.client后注册,1.0则不需要
chat.client.broadcastMessage = function (guid, count, Num, remark, headimgurl) {
    LoadBag(guid, count, Num, remark, headimgurl);
    bag.rID = guid;
    bag.userId = UserInfo.openid;
    bag.bagAmount = count;
    bag.bagNum = Num;
    bag.headimgUrl = UserInfo.headimgurl;
};
chat.client.loadUserCountMessage = function (count) {
    $('#title').html("金豆一群 (" + count + ") ");
}
chat.client.loadAmtMessage = function (openid, imgurl) {
    var html = "";
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    html += "<li><p class=\"am-text-center cf f12\">" + h + ":" + m + "</p>";
    html += "  <div class=\"oz\"><div class=\"right\">";
    html += "                    <img src=\"" + imgurl + "\" /></div>";
    html += "   <div style=\"float:right;margin-right:10px\">";
    html += "      <img src=\"" + Apiurl + "QRFile/" + openid + ".jpg\" style=\"width:210px;\" /> ";
    html += "   </div></li>";
    $("#msg").append(html);
    $('#contentArea').scrollTop($('.bd').height());

};
chat.client.loadImgMessage = function (typeid, userID, imgurl, nickname) {
    var html = "";
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    var type = "";
    if (typeid == "1") {
        type = "start1";
    }
    if (typeid == "2") {
        type = "stop1";
    }
    if (typeid == "3") {
        type = "any";
    }
    if (typeid == "4") {
        type = "speend";
    }
    if (typeid == "5") {
        type = "next";
    }
    if (typeid == "6") {
        type = "ready";
    }
    html += "<li><p class=\"am-text-center cf f12\">" + h + ":" + m + "</p>";
    html += "  <div class=\"oz\"><div class=\"right\">";
    html += "                    <img src=\"" + imgurl + "\" /></div>";
    html += "   <div style=\"float:right;margin-right:10px\">";
    if (typeid == "1" || typeid == "2") {
        html += "      <img src=\"" + Weburl + "image/" + type + ".gif\" style=\"width:100px;\" /> ";
        MessageRecord.imgUrl = Weburl + "image/" + type + ".gif";
    }
    else {
        html += "      <img src=\"" + Weburl + "image/" + type + ".png\" style=\"width:100px;\" /> ";
        MessageRecord.imgUrl = Weburl + "image/" + type + ".png";
    }

    html += "   </div></li>";
    $("#msg").append(html);
    $('#contentArea').scrollTop($('.bd').height());


};
chat.client.loadMessage = function (mid, nickname, message, userImg, curUser, time, type) {
    var html = "";
    var curTime = time;
    time = time.split(" ")[1].split(":");
    //if (data.code == "SCCESS") {
    if (type == 0) {
        html += "<li id=\"li_" + mid + "\"><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
        html += " <div class=\"right\" style=\"width:20%\">";
        html += "                     <a href=\"javascript:;\" onclick=\"return false;\"><img src=\"" + userImg + "\" style=\"width:3.5em;height:3.5em\"/></a>";
        html += "                  </div>";
        html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
        html += "        <span class=\"bubble rightBubble\" id=\"" + mid + "\" onclick=\"showCancel('" + mid + "','" + nickname + "',this,'" + curTime + "')\" style=\"max-width:70%\">";
        html += "          <div id=\"div_" + mid + "\">" + message + "</div>";
        html += "            <span class=\"bottomLevel\"></span>";
        html += "           <span class=\"topLevel\"></span>";
        html += "        </span>";
        html += "   </div></li>";
    }
    else {
        $("#li_" + mid).empty();
        var html = "";
        html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
        html += "   <div>   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
        html += "        <span  id=\"" + mid + "\" style=\"max-width:70%\">";
        html += "          <div id=\"div_" + mid + "\" style=\"background:#E6E6E6;border-radius:15px;width:60%;margin-left:60px;font-family:黑体;font-size:12px;color:white\">" + nickname + "撤回了一条消息。</div>";
        html += "            <span class=\"bottomLevel\"></span>";
        html += "           <span class=\"topLevel\"></span>";
        html += "        </span>";
        html += "   </div></li>";
        $("#li_" + mid).html(html);
    }
    //}
    //else {
    //    html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
    //    html += " <div class=\"oz\">";
    //    html += " <div class=\"right\">";
    //    html += "<a href=\"javascript:void(0);\" ><img src=\"" + userImg + "\" /></a>";
    //    html += "</div>";
    //    html += "<div class=\"cont_right\">";
    //    html += "<a class=\"cf\" href=\"javascript:void(0);\" onclick=\"OpenBag('" + data.bagID + "','" + data.bagUserID + "');\">";
    //    html += "<div>" + data.bagRemark + " </div>";
    //    html += "</a>";
    //    html += "</div>";
    //    html += "</div> </li>";
    //}
    if (type == 0) {
        $("#msg").append(html);
    }
    $('#contentArea').scrollTop($('.bd').height());


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

};

// 启动连接,这里和1.0也有区别
$.connection.hub.start().done(function () {
    $.ajax({
        url: Apiurl + "api/test/getUsercount", // url  action是方法的名称
        type: "Get",
        //data: { bagId: openid },
        //async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.result != null) {
                chat.server.userCount(data.result);
            }
        }
    });
    $.ajax({
        url: Apiurl + "api/test/getWxUser", // url  action是方法的名称
        type: "Get",
        data: { bagId: openid },
        //async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code == "SCCESS") {
                if (data.result != null) {
                    UserInfo.city = data.result.city;
                    UserInfo.country = data.result.country;
                    UserInfo.headimgurl = data.result.headimgurl;
                    UserInfo.nickname = data.result.nickname;
                    UserInfo.openid = data.result.openid;
                    UserInfo.privilege = data.result.privilege;
                    UserInfo.province = data.result.province;
                    UserInfo.sex = data.result.sex;
                    UserInfo.unionid = data.result.unionid;
                    UserInfo.invite = data.result.invite;
                    UserInfo.treaty = data.result.treaty;
                    if (data.result.invite != 1) {
                        layer.msg("您没有接受邀请，不能进入群聊");
                        wx.closeWindow();
                    }
                    else {
                        layer.closeAll();
                        //询问框
                        if (UserInfo.treaty != 1) {
                            showMZSM(UserInfo.openid);
                        }

                        $.ajax({
                            url: Apiurl + "api/test/getmsglist", // url  action是方法的名称
                            type: "Get",
                            data: { time: "", pageIndex: 1 },
                            //async: false,
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
                                        html += "<li id=\"li_" + item.mid + "\"><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                                        html += " <div class=\"right\" style=\"width:20%\">";
                                        html += "                     <a href=\"javascript:;\" onclick=\"return false;\"><img src=\"" + item.headImgUrl + "\" style=\"width:3.5em;height:3.5em\"/></a>";
                                        html += "                  </div>";
                                        html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
                                        html += "        <span class=\"bubble rightBubble\" id=\"" + item.mid + "\" onclick=\"showCancel('" + item.mid + "','" + item.nickname + "',this,'" + item.createTime + "')\" style=\"max-width:70%\">";
                                        html += "          <div id=\"div_" + item.mid + "\">" + item.mContent + "</div>";
                                        html += "            <span class=\"bottomLevel\"></span>";
                                        html += "           <span class=\"topLevel\"></span>";
                                        html += "        </span>";
                                        html += "   </div></li>";
                                    }
                                    else if (item.mType == 1) {
                                        html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                                        html += " <div class=\"oz\">";
                                        html += " <div class=\"right\" style=\"width:20%\">";
                                        html += "<a href=\"javascript:void(0);\" onclick=\"return false;\" ><img src=\"" + item.headImgUrl + "\"  style=\"width:3.5em;height:3.5em\"/></a>";
                                        html += "</div>";
                                        html += "<div class=\"cont_right\">";
                                        html += "<a class=\"cf\" href=\"javascript:void(0);\" onclick=\"OpenBag('" + item.bagID + "','" + item.bagUserID + "');\">";
                                        html += "<div>" + item.bagRemark + " </div>";
                                        html += "</a>";
                                        html += "</div>";
                                        html += "</div> </li>";
                                    }
                                    else if (item.mType == 2) {
                                        html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                                        html += "  <div class=\"oz\"><div class=\"right\" style=\"width:20%\">";
                                        html += "                    <img src=\"" + item.amtUserImg + "\"  style=\"width:3.5em;height:3.5em\"/></div>";
                                        html += "   <div style=\"float:right;margin-right:10px\">";
                                        html += "      <img src=\"" + Apiurl + "QRFile/" + item.amtUserID + ".jpg\" style=\"width:180px;\" /> ";
                                        html += "   </div></li>";

                                    }
                                    else if (item.mType == 3) {
                                        html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                                        html += "  <div class=\"oz\"><div class=\"right\">";
                                        html += "                    <img src=\"" + item.headImgUrl + "\" /></div>";
                                        html += "   <div style=\"float:right;margin-right:10px\">";
                                        html += "      <img src=\"" + item.imgUrl + "\" style=\"width:100px;\" /> ";
                                        html += "   </div></li>";

                                    }
                                    else if (item.mType == 5) {
                                        html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                                        html += "   <div>   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
                                        html += "        <span  id=\"" + item.mid + "\" style=\"max-width:70%\">";
                                        html += "          <div id=\"div_" + item.mid + "\" style=\"background:#E6E6E6;border-radius:15px;width:60%;margin-left:60px;font-family:黑体;font-size:12px;color:white\">" + item.mContent + "</div>";
                                        html += "            <span class=\"bottomLevel\"></span>";
                                        html += "           <span class=\"topLevel\"></span>";
                                        html += "        </span>";
                                        html += "   </div></li>";
                                    }
                                });
                                $("#msg").append(html);
                                if ($("#initHeight").val() == "") {
                                    $("#initHeight").val($(window).height());
                                }

                                var initheight = parseInt($("#initHeight").val()) - $(".shurukuang").height() - $("h2").height();
                                $("#contentArea").css("height", initheight);
                                $('#contentArea').scrollTop($('.bd').height());
                            }
                        });
                    }
                }
            }
            else {
                layer.msg("您没有接受邀请，不能进入群聊");
                wx.closeWindow();
            }
        }
    });

    $('#getAmt').click(function () {
        if (UserInfo.openid == null) {
            UserInfo.openid = openid;
        }
        $.ajax({
            url: Apiurl + "api/test/getAmtCode", // url  action是方法的名称
            type: "Get",
            data: { bagId: UserInfo.openid },
            async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                if (data.code == "SCCESS") {
                    if (data.result.hasImg == 0) {
                        layer.msg("还没有上传收款码，请先上传收款码！");
                        return;
                    }
                }
                var headUrl = UserInfo.headimgurl;
                var userID = UserInfo.openid;


                MessageRecord.mID = newGuid();
                MessageRecord.mType = 2;//收款图片
                MessageRecord.amtUserID = userID;
                MessageRecord.userID = userID;
                MessageRecord.amtUserImg = UserInfo.headimgurl;
                MessageRecord.nickname = UserInfo.nickname;
                var dataJson = JSON.stringify(MessageRecord);
                $.ajax({
                    url: Apiurl + "api/test/istText", // url  action是方法的名称
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
                        //resdata = data;
                        chat.server.sendAmtMessage(userID, headUrl);
                    }
                });

            }
        });
    });

});
function sendImg(type,cls) {
    $("."+cls).setBtnTimer({
        'time': 2
    });
    var headUrl = UserInfo.headimgurl;
    $("#textmsg").val("");
    chat.server.sendImgMessage(type, UserInfo.openid, headUrl, UserInfo.nickname);
    MessageRecord.mID = newGuid();
    MessageRecord.mType = 3;//收款图片
    MessageRecord.imgUserID = UserInfo.openid;
    MessageRecord.headImgUrl = UserInfo.headimgurl;
    MessageRecord.nickname = UserInfo.nickname;
    var types = "";
    if (type == "1") {
        types = "start1";
    }
    if (type == "2") {
        types = "stop1";
    }
    if (type == "3") {
        types = "any";
    }
    if (type == "4") {
        types = "speend";
    }
    if (type == "5") {
        types = "next";
    }
    if (type == "6") {
        types = "ready";
    }
    if (type == "1" || type == "2") {
        MessageRecord.imgUrl = Weburl + "image/" + types + ".gif";
    }
    else {
        MessageRecord.imgUrl = Weburl + "image/" + types + ".png";
    }
    var dataJson = JSON.stringify(MessageRecord);
    $.ajax({
        url: Apiurl + "api/test/istText", // url  action是方法的名称
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
            //resdata = data;
            // chat.server.sendAmtMessage(userID, headUrl);
        }
    });
}
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
    $('#sendMsg').click(function () {

        var message = $("#textmsg").val();
        if (message == "") {
            layer.msg("不能发送空消息！");
            return;
        }

        MessageRecord.mID = newGuid();
        MessageRecord.mContent = message;
        MessageRecord.mType = 0;
        MessageRecord.userID = UserInfo.openid;
        MessageRecord.headImgUrl = UserInfo.headimgurl;
        MessageRecord.nickname = UserInfo.nickname;
        var dataJson = JSON.stringify(MessageRecord);
        $.ajax({
            url: Apiurl + "api/test/istText", // url  action是方法的名称
            type: "Post",
            data: dataJson,
            async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                $("#textmsg").val("");
                chat.server.sendMessage(MessageRecord.mID, UserInfo.nickname, message, UserInfo.headimgurl, UserInfo.openid, 0);
            }
        });

    });
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
    //$(".bubble").click(function () {
    //    alert("11");
    //})
    /*设置一个长按的计时器，如果点击这个图层超过2秒则触发，mydiv里面的文字从out变in的动作*/

});
function OpenBag(guid, customerCode) {
    $.ajax({
        url: Apiurl + "api/test/getHasBag", // url  action是方法的名称
        type: "Get",
        data: { bagId: guid.toUpperCase(), userId: UserInfo.openid },
        // async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            CurUserBag = data.result;
            console.log(CurUserBag);
            var index;
            if (data.code == "SCCESS") {
                index = layer.open({
                    type: 2,
                    content: '../Main/openbag.html?v=1.0',
                    area: ['320px', '195px'],
                    maxmin: false,
                    closeBtn: 0,
                    title: "",
                    success: function (layero, index) {
                        var body = layer.getChildFrame('body', index);
                        var iframeWin = window[layero.find('iframe')[0]['name']]; //得到iframe页的窗口对象，执行iframe页的方法：iframeWin.method();
                        body.find(':input[type=hidden]').val(guid.toUpperCase() + "|" + UserInfo.openid);
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
                        body.find(':input[type=hidden]').val(guid + "|" + customerCode);
                    }
                });
            }
            layer.full(index);
        }
    });
    //var ret = callBackTwoDataFunc("api/test/getHasBag", guid, customerCode, "");

}
function LoadBag(guid, count, Num, remark, headimgurl) {
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    var bag = " <li><p class=\"am-text-center cf f12\">" + h + ":" + m + "</p>";
    bag += " <div class=\"oz\">";
    bag += " <div class=\"right\" style=\"width:20%\">";
    bag += "<a href=\"javascript:void(0);\" ><img src=\"" + headimgurl + "\" style=\"width:3.5em;height:3.5em\" /></a>";
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
function showCancel(mid, nickname, ctl, time) {
    MessageRecord.mID = mid;
    MessageRecord.userID = UserInfo.openid;
    var dataJson = JSON.stringify(MessageRecord);
    $.ajax({
        url: Apiurl + "api/test/showcancelmenu", // url  action是方法的名称
        type: "Post",
        data: dataJson,
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code == "SCCESS") {
                layer.tips('<a href="javascript:;" style=\"color:white\" onclick=\'cancelMsg("' + mid + '","' + nickname + '")\'>撤回</a>', ctl, {
                    tips: [1, 'black'] //还可配置颜色
                });
            }
        }
    });
}
function cancelMsg(mid, nickname) {
    var index = layer.confirm('是否要撤回？', {
        btn: ['确定', '取消'] //按钮
    }, function () {
        MessageRecord.mID = mid;
        MessageRecord.mContent = nickname + "撤回了一条消息。";
        var dataJson = JSON.stringify(MessageRecord);
        $.ajax({
            url: Apiurl + "api/test/cancelMsg", // url  action是方法的名称
            type: "Post",
            data: dataJson,
            async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                chat.server.sendMessage(MessageRecord.mID, UserInfo.nickname, MessageRecord.mContent, UserInfo.headimgurl, UserInfo.openid, 5);
                layer.closeAll();
            }
        });
    }, function () {

    });

}
function showMZSM(openid) {
    var html = "";
    html += "<div >&nbsp;&nbsp;&nbsp;&nbsp;本网站所提供的信息，只供参考之用。<br/>";

    html += "&nbsp;&nbsp;&nbsp;&nbsp;本网站及其雇员一概毋须以任何方式就任何信息传递或传送的失误、不准确或错误对用户或任何其他人士负任何直接或间接的责任。<br/>";

    html += " &nbsp;&nbsp;&nbsp;&nbsp;在法律允许的范围内，本网站在此声明,不承担用户或任何人士就使用或未能使用本网站所提供的信息或任何链接或项目所引致的任何直接、间接、附带、从属、特殊、惩罚性或惩戒性的损害赔偿（包括但不限于收益、预期利润的损失或失去的业务、未实现预期的节省）。<br/>";

    html += " &nbsp;&nbsp;&nbsp;&nbsp;本网站所提供的信息，若在任何司法管辖地区供任何人士使用或分发给任何人士时会违反该司法管辖地区的法律或条例的规定或会导致本网站或其第三方代理人受限于该司法管辖地区内的任何监管规定时，则该等信息不宜在该司法管辖地区供该等任何人士使用或分发给该等任何人士。用户须自行保证不会受限于任何限制或禁止用户使用或分发本网站所提供信息的当地的规定。<br/>";

    html += "&nbsp;&nbsp;&nbsp;&nbsp;本网站图片，文字之类版权申明，因为网站可以由注册用户自行上传图片或文字，本网站无法鉴别所上传图片或文字的知识版权，如果侵犯，请及时通知我们，本网站将在第一时间及时删除。<br/>";

    html += " &nbsp;&nbsp;&nbsp;&nbsp;凡以任何方式登陆本网站或直接、间接使用本网站资料者，视为自愿接受本网站声明的约束。</div>";
    layer.confirm(html, {
        btn: ['接受', '拒绝'], //按钮
        title: "免责声明",
        closeBtn: 0
    }, function () {
        $.ajax({
            url: Apiurl + "api/test/agreetreaty", // url  action是方法的名称
            type: "Get",
            data: { openid: openid, id: 1 },
            async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            success: function (data) {
                layer.closeAll();
            }
        });
    }, function () {
        wx.closeWindow();
    });
}
$.fn.setBtnTimer = function (options) {
    "use strict";
    var defaults = {
        'time': 60
    };
    var settings = $.extend({}, defaults, options);
    this.attr("disabled", "true");
    var that = this,
        oldv = that.val(),
        timer,
        tick = function () {
            settings.time--;
            that.val(settings.time + 's后可在此操作');
            if (settings.time < 1) {
                that.removeAttr("disabled");
                window.clearInterval(timer);
                that.val(oldv);
            }
        };
    return this.each(function () {
        timer = window.setInterval(tick, 1000);
    });
}
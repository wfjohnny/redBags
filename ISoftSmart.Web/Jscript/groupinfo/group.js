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
    unionid: "",
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
    imgUrl:""

};
var openid;
//if (getUrlParam("openid") == null) {
//    openid = "olDlVsy5vYjAhbWIDMYaj5PSVp04";
//}
//else {
openid = getUrlParam("oid");
//}
var wxUserres;
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
        if (data.code) {
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

                if (data.result.invite != 1) {
                    layer.msg("您没有接受邀请，不能进入群聊");
                    wx.closeWindow();
                }
            }
        }
    }
});
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
            $('#title').html("金豆一群 (" + data.result + ") ");
        }

    }
});

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
chat.client.loadImgMessage = function (typeid,userID, imgurl) {
    var html = "";
    var myDate = new Date();
    var h = myDate.getHours();
    var m = myDate.getMinutes();
    var type = "";
    if (typeid == "1") {
        type = "start";
    }
    if (typeid == "2") {
        type = "stop";
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
    debugger
    html += "<li><p class=\"am-text-center cf f12\">" + h + ":" + m + "</p>";
    html += "  <div class=\"oz\"><div class=\"right\">";
    html += "                    <img src=\"" + imgurl + "\" /></div>";
    html += "   <div style=\"float:right;margin-right:10px\">";
    html += "      <img src=\"" + Weburl + "image/" + type + ".png\" style=\"width:100px;\" /> ";
    html += "   </div></li>";
    $("#msg").append(html);
    $('#contentArea').scrollTop($('.bd').height());
    debugger
    MessageRecord.mID = newGuid();
    MessageRecord.mType = 3;//收款图片
    MessageRecord.imgUserID = userID;
    MessageRecord.headImgUrl = imgurl;
    MessageRecord.imgUrl =Weburl + "image/" + type + ".png";
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

};
chat.client.loadMessage = function (message, userImg, curUser, time) {
    var html = "";
    time = time.split(" ")[1].split(":");
    //if (data.code == "SCCESS") {
    html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
    html += " <div class=\"right\" style=\"width:20%\">";
    html += "                     <a href=\"javascript(void:0);\"><img src=\"" + userImg + "\" style=\"width:3.5em;height:3.5em\"/></a>";
    html += "                  </div>";
    html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
    html += "        <span class=\"bubble rightBubble\" style=\"max-width:70%\">";
    html += "            " + message + "";
    html += "            <span class=\"bottomLevel\"></span>";
    html += "           <span class=\"topLevel\"></span>";
    html += "        </span>";
    html += "   </div></li>";
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
    $("#msg").append(html);
    $("#textmsg").val("");
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
                    html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                    html += " <div class=\"right\" style=\"width:20%\">";
                    html += "                     <a href=\"javascript(void:0);\"><img src=\"" + item.headImgUrl + "\"  style=\"width:3.5em;height:3.5em\"/></a>";
                    html += "                  </div>";
                    html += "   <div class=\"bubbleItem clearfix\">   <span style=\"font-family: Arial, Helvetica, sans-serif;\"><!--右侧的泡泡--></span>";
                    html += "        <span class=\"bubble rightBubble\" style=\"max-width:70%\">";
                    html += "            " + item.mContent + "";
                    html += "            <span class=\"bottomLevel\"></span>";
                    html += "           <span class=\"topLevel\"></span>";
                    html += "        </span>";
                    html += "   </div></li>";
                }
                else if (item.mType == 1) {
                    html += " <li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                    html += " <div class=\"oz\">";
                    html += " <div class=\"right\" style=\"width:20%\">";
                    html += "<a href=\"javascript:void(0);\" ><img src=\"" + item.headImgUrl + "\"  style=\"width:3.5em;height:3.5em\"/></a>";
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
                else if (item.mType ==3) {
                    html += "<li><p class=\"am-text-center cf f12\">" + time[0] + ":" + time[1] + "</p>";
                    html += "  <div class=\"oz\"><div class=\"right\">";
                    html += "                    <img src=\"" + item.headImgUrl + "\" /></div>";
                    html += "   <div style=\"float:right;margin-right:10px\">";
                    html += "      <img src=\"" +item.imgUrl+"\" style=\"width:100px;\" /> ";
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
function sendImg(type) {
    var headUrl = UserInfo.headimgurl;
    chat.server.sendImgMessage(type,UserInfo.openid, headUrl);
}
//$('#start').click(function () {

//})
//$("#stop").click(function () {
//    var headUrl = UserInfo.headimgurl;
//    chat.server.sendImgMessage("2", headUrl);
//})
//$("#any").click(function () {
//    var headUrl = UserInfo.headimgurl;
//    chat.server.sendImgMessage("3", headUrl);
//})
//$("#speend").click(function () {
//    var headUrl = UserInfo.headimgurl;
//    chat.server.sendImgMessage("4", headUrl);
//})
//$("#next").click(function () {
//    var headUrl = UserInfo.headimgurl;
//    chat.server.sendImgMessage("5", headUrl);
//})
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
                chat.server.sendMessage(message, UserInfo.headimgurl, UserInfo.openid);
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


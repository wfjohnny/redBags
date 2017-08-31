//var customerNick = "Customer" + Math.random().toString().substring(2, 5);
//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg); //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}
$(function () {

    if (getUrlParam("code") == null && getUrlParam("state") == null) {
        var RetUrl = "http://www.isoftsmart.com/bagWeb/Authorization/auth.html";
        var wxUrls = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + AppId + "&redirect_uri=" + RetUrl + "&response_type=code&scope=snsapi_base&state=1#wechat_redirect";
        window.location.href = wxUrls;
    }
    else {
        var code = getUrlParam("code");
        var wxUrls = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + AppId + "&secret=" + AppSecret + "";
        var ret = callBackWeChatFunc("api/test/wxGetUserInfo", wxUrls, code, "");
        var myDate = new Date();
        var h = myDate.getHours();
        var m = myDate.getMinutes();
        window.location.href = HomeUrl + "?oid=" + ret.result.openid;
    }
});
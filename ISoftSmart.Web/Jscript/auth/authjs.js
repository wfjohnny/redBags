/// <reference path="H:\Code\src\ISoftSmart\ISoftSmart.Web\init/initData.js" />
$(document).ready(function () {
    //获取授权用户信息
    var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + APPID + "&redirect_uri=" + WebPageUrl + "RedEnvelope.htm" + "&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";
    var res = callBackOtherUrlJson(url, "");
    console.log(res);
});
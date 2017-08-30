$(function () {
    $("#close").click(function () {
        parent.layer.closeAll("iframe");
    });
    $("#upload").click(function () {
        index = layer.open({
            type: 2,
            content: '../Main/uploadfile.html',
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
    $.ajax({
        url: Apiurl + "api/test/getUserInfocount", // url  action是方法的名称
        type: "Get",
        // data: { bagId: bagId },
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (res) {
            if (res.code == "SCCESS") {
                var html = "";
                $(res.result).each(function (a, item) {
                    html += "<div style=\"width:65px;float:left;margin:0 auto;text-align:center\">";
                    if (item.headimgurl == "") {
                        html += "<img src=\"../Jscript/hongbao/images/biaoqing.png\" style=\"width:40px;border-radius: 40px\" /><br/>";
                    }
                    else {
                        html += "<img src=\"" + item.headimgurl + "\" style=\"width:40px;border-radius: 40px\" /><br/>";
                    }

                    html += "<lable style=\"font-size:9px;\">" + item.nickname + "</lable>";
                    html += "</div>";
                });
                $(".to").append(html);
            }
            else {

            }
        }
    });
    wx.config({
        debug: true, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
        appId: AppId, // 必填，公众号的唯一标识
        timestamp: , // 必填，生成签名的时间戳
        nonceStr: '', // 必填，生成签名的随机串
        signature: '',// 必填，签名，见附录1
        jsApiList: [] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
    });
    $("#sharepage").click(function () {
        wx.onMenuShareAppMessage({
            title: '', // 分享标题
            desc: '', // 分享描述
            link: '', // 分享链接，该链接域名或路径必须与当前页面对应的公众号JS安全域名一致
            imgUrl: '', // 分享图标
            type: '', // 分享类型,music、video或link，不填默认为link
            dataUrl: '', // 如果type是music或video，则要提供数据链接，默认为空
            success: function () {
                // 用户确认分享后执行的回调函数
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            }
        });
    });
});
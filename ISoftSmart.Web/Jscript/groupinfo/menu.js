/// <reference path="sharebox.js" />



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


});

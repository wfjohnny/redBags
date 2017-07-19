$(function () {
    $.ajax({
        url: "http://localhost:8931/api/test/t", // url  action是方法的名称
        type: "Get",
        async: false,
        xhrFields: {
            withCredentials: true
        },
        crossDomain: true,//新增cookie跨域配置
        dataType: "json",
        contentType: "application/json",
        success: function (data) {
            if (data.code == "SUCCESS") {
                console.log(data);
            }
            else {
            }
        }
    });
});
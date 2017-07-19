$(document).ready(function () {

    $("#btnsignin").click(function () {
        var aj = $.ajax({
            url: '/Home/SignIn',// 跳转到 action  
            data: {
                username: $("#username").val(),
                password: $("#password").val()
            },
            type: 'post',
            cache: false,
            dataType: 'json',
            success: function (data) {

                if (data != null) {
                    window.location = "Home/Index";
                }
                else {
                    alert("用户名或密码错误！");
                }
            },
            error: function () {
                alert("用户不存在！");
            }
        });
    });
});
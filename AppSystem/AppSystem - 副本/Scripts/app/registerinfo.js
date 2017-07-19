$(function () {
    $("#backsubmit").click(function () {
        Patients.name = $("#name").val();
        Patients.sex = $("#sex").val();
        Patients.birthday = $("#birth").val();
        Patients.address = $("#address").val();
        Patients.nowaddress = $("#faddress").val();
        Patients.moblie = $("#mobilep").val();
        Patients.facecard = facecardimg;
        Patients.backcard = backcardimg;
        Patients.idcard = $("#idcard").val();
        Patients.national = $("#national").val();
        console.log(JSON.stringify(Patients));
        $.ajax({
            type: "POST",
            url: serverBase + "api/patients/insertpatient",
            async: false,
            xhrFields: {
                withCredentials: true
            },
            crossDomain: true,//新增cookie跨域配置
            dataType: "json",
            contentType: "application/json",
            data: JSON.stringify(Patients),
            async: false,
            success: function (src) {
                if (src.code == "SUCCESS")
                {
                    alert(src.message);
                    location.href = "/Home/Index";
                }
            }
        });
        //location.href = "/home/index";
    })
})
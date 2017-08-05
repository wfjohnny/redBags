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
});
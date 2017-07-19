var customerNick = "Customer" + Math.random().toString().substring(2, 5);
$(function () {
    //读取cookie
    if (jQuery.cookie("customerCookie")) {
        customerCode = jQuery.cookie("customerCookie");
    } else {
        customerCode = customerNick;
    }
    //设置cookie
    jQuery.cookie("customerCookie", customerCode, {
        expires: 3
    });
   // 添加数据
    //$.session.set('userCode', 'value');

   // 删除数据
    //$.session.remove('key');

   // 获取数据
    //$.session.get('key');

   // 清除数据
    //$.session.clear();
});
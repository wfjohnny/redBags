using ISoftSmart.Core.IoC;
using ISoftSmart.Core.WebApi;
using ISoftSmart.Inteface;
using ISoftSmart.Inteface.Implements;
using ISoftSmart.Inteface.Inteface;
using ISoftSmart.Model;
using ISoftSmart.Model.AD;
using ISoftSmart.Model.BS;
using ISoftSmart.Model.RB;
using ISoftSmart.Model.UserInfo;
using ISoftSmart.Model.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISoftSmart.API.Init
{
    public static class InitConfig
    {
        public static void InitIoC()
        {
            List<ModelMaster> list = new List<ModelMaster>() {
                new ModelMaster() { from = typeof(IRedBag), to = typeof(RedBagExtends) },
                new ModelMaster() { from = typeof(ITestUsers), to = typeof(UserExtents) },


                new ModelMaster() { from = typeof(AdUser) },
                new ModelMaster() { from = typeof(RBCreateBag) },
                new ModelMaster() { from = typeof(UserInfo) },
                new ModelMaster() { from = typeof(APIClient) },
                new ModelMaster() { from = typeof(WXUserInfo) },
                 //new ModelMaster() { from = typeof(List<BagSerial>) },

            };
            IoCFactory.Instance.CurrentContainer.RegisterTypeList(list);//注册接口

        }

    }

}
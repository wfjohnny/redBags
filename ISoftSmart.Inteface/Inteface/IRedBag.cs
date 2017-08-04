﻿using ISoftSmart.Model.BS.My;
using ISoftSmart.Model.RB;
using ISoftSmart.Model.UserInfo;
using ISoftSmart.Model.WX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Inteface.Inteface
{
    public interface IRedBag :IDependency
    {
        List<RBCreateBag> GetBag(RBCreateBag bag);
        int ChangeBagStatus(RBCreateBag bag);
        int InsertBag(RBCreateBag bag);
        int SetUserBean(WXUserInfo bag);
        List<MyBagSerial> GetUserSerialList(MyBagSerial my);
        int InsertSerial(MyBagSerial my);
        int InsertSerialList(List<MyBagSerial> my);
        List<RBCreateBag> GetSendBag(RBCreateBag bag);
        List<WXUserInfo> GetUserInfo(WXUserInfo user);
        int InsertUserInfo(WXUserInfo user);
        int InsertMessageRecordByText(MessageRecord message);
        int InsertMessageRecordByBag(MessageRecord msg);
        List<MessageRecord> GetMsgList(DateTime startTime, DateTime endTime);
    }
}

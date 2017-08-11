using ISoftSmart.API.App_Start;
using ISoftSmart.Common.Log;
using ISoftSmart.Core.IoC;
using ISoftSmart.Core.Logger;
using ISoftSmart.Core.RedisClient;
using ISoftSmart.Core.WebApi;
using ISoftSmart.Core.WebApi.APIServer.Request;
using ISoftSmart.Inteface.Implements;
using ISoftSmart.Inteface.Inteface;
using ISoftSmart.Model;
using ISoftSmart.Model.BS;
using ISoftSmart.Model.BS.My;
using ISoftSmart.Model.RB;
using ISoftSmart.Model.UserInfo;
using ISoftSmart.Model.WX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using Newtonsoft.Json;
using StackExchange.Redis;
using ISoftSmart.Common.UploadHelper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace ISoftSmart.API.Controllers
{
    [RoutePrefix("api/test")]
    public class HomeController : BaseController, IRequiresSessionState
    {
        private string IMG_BASE64;
        private static object _locker = new object();
        string AppId = System.Configuration.ConfigurationManager.AppSettings["AppId"].ToString();
        string AppSecret = System.Configuration.ConfigurationManager.AppSettings["AppSecret"].ToString();
        string RetUrl = System.Configuration.ConfigurationManager.AppSettings["RetUrl"].ToString();
        string GetUser = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}";
        string GetOpenID = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        int pageSize = 100;
        IDatabase db = RedisManager.Instance.GetDatabase();
        [Route("t")]
        [HttpGet]
        public IHttpActionResult Index(string bagId)
        {
            #region IOC
            // ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.RegisterType(typeof(ITestUsers), typeof(UserExtents));//注册接口
            //var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<ITestUsers>();//使用接口
            //var tt = rt.Test2();//执行SQL返回JSON
            #endregion
            #region Redis
            //var db = RedisManager.Instance.GetDatabase();
            ////var result2 =  (int)RedisManager.Instance.GetDatabase().StringGet("abc123zzl");
            ////StackExchangeRedisExtensions.Set(db, "t", "testtttt");//操作字符
            //var usr = new UserInfo()
            //{
            //    Age = 1,
            //    ID = "2",
            //    Name = "22222"
            //};
            //List<UserInfo> list = new List<UserInfo>();
            //list.Add(usr);//生成一个list
            //StackExchangeRedisExtensions.Set(db, "t", list);//设置值
            //var getDbVal = StackExchangeRedisExtensions.Get<List<UserInfo>>(db, "t");
            //var usr1 = new UserInfo()
            //{
            //    Age = 1,
            //    ID = "33332",
            //    Name = "2222333的2"
            //};
            //getDbVal.Add(usr1);
            //StackExchangeRedisExtensions.Set(db, "t", getDbVal);//设置值
            ////StackExchangeRedisExtensions.Append(db, "t", usr);
            //var getDbVal1 = StackExchangeRedisExtensions.Get(db, "t");
            //var getkey = StackExchangeRedisExtensions.HasKey(db, "t1");
            ////StackExchangeRedisExtensions.Remove(db, "t");

            #endregion
            #region Redis队列
            //RedisQueueManager.Push()
            #endregion

            //var rt = IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//注册对象
            //var bag = IoCFactory.Instance.CurrentContainer.Resolve<RBCreateBag>();//注册对象
            //bag = rt.GetBag(bag);
            var rt = IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//注册对象
            Guid rId = Guid.Parse(bagId.Split('_')[0]);
            try
            {
                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.BagKey))
                {
                    var bagcaches = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                    var bagList = bagcaches.Where(x => x.RID == rId).FirstOrDefault();
                    if (bagList != null)
                    {
                        return Ok(new APIResponse<RBCreateBag>
                        {
                            Code = "SUCCESS",
                            ResponseMessage = "获取列表成功！",
                            Result = bagList
                        });
                    }
                    else
                    {
                        var bag = new RBCreateBag();//注册对象
                        List<RBCreateBag> bList = new List<RBCreateBag>();
                        bag.BagStatus = 0;
                        bag.RID = Guid.Parse(bagId.Split('_')[0]);
                        bList = rt.GetBag(bag);
                        return Ok(new APIResponse<RBCreateBag>
                        {
                            Code = "SCCESS",
                            ResponseMessage = "获取列表成功！",
                            Result = bList.FirstOrDefault()
                        });
                    }
                }
                else
                {
                    var bag = IoCFactory.Instance.CurrentContainer.Resolve<RBCreateBag>();//注册对象
                    List<RBCreateBag> bagList = new List<RBCreateBag>();
                    bag.BagStatus = 0;
                    bag.RID = Guid.Parse(bagId.Split('_')[0]);
                    bagList = rt.GetBag(bag);
                    if (bagList != null)
                    {
                        bagList.Add(bag);
                        StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bagList);
                        var bagcaches = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                        return Ok(new APIResponse<List<RBCreateBag>>
                        {
                            Code = "SUCCESS",
                            ResponseMessage = "获取列表成功！",
                            Result = bagcaches
                        });
                    }
                    else
                    {
                        return Ok(new APIResponse<RBCreateBag>
                        {
                            Code = "ERROR",
                            ResponseMessage = "红包抢完了！",
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<string>
                {
                    Code = "ERROR",
                    ResponseMessage = "抢红包出错！",
                    Result = ex.Message
                });
            }
        }

        [Route("getWxUser")]
        [HttpGet]
        public IHttpActionResult GetUserInfoByWX(string bagId)
        {
            try
            {
                var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
                var userinfoList = rt.GetUserInfo(new WXUserInfo() { openid = bagId }).FirstOrDefault();
                return Ok(new APIResponse<WXUserInfo>
                {
                    Code = "SCCESS",
                    ResponseMessage = "获取用户信息成功",
                    Result = userinfoList
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<WXUserInfo>
                {
                    Code = "SCCESS",
                    ResponseMessage = "获取用户信息失败",
                    Result = null
                });
            }
        }
        [Route("openbag")]
        [HttpPost]
        public IHttpActionResult OpenBag(RBCreateBag bag)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
            //bag.CreateTime = DateTime.Now;
            //var res = await Task.Run(() =>rt.GetBag(bag));
            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            RBCreateBag Result = null;
            //Task.Run(() =>
            //{
            bag.CreateTime = DateTime.Now;
            var outbag = IoCFactory.Instance.CurrentContainer.Resolve<RBCreateBag>();//注册对象
            try
            {
                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.BagKey))
                {
                    lock (_locker)
                    {
                        var bagcache = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                        var tr = bagcache.Where(x => x.RID == bag.RID).FirstOrDefault();
                        if (tr != null && tr.BagNum > 0)
                        {
                            decimal curAmount = 0;
                            var openResult = GenerateBag(tr, out outbag, out curAmount);
                            bagcache.Remove(bagcache.Where(x => x.RID == outbag.RID).FirstOrDefault());
                            bagcache.Add(outbag);
                            Code = "SUCCESS";
                            ResponseMessage = "抢到" + curAmount + "个金豆！";
                            Result = openResult;
                            StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bagcache);
                            var userInfo = IoCFactory.Instance.CurrentContainer.Resolve<WXUserInfo>();//注册对象
                            userInfo.beannum = curAmount;
                            userInfo.openid = bag.UserId;
                            rt.SetUserBean(userInfo);

                            MyBagSerial bsent = new MyBagSerial();//增加到流水表
                            bsent.SerialId = Guid.NewGuid();
                            bsent.UserId = bag.UserId;
                            bsent.BagAmount = curAmount;
                            bsent.CreateTime = DateTime.Now;
                            bsent.RID = bag.RID;
                            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.SerialKey))
                            {
                                var bagkey = StackExchangeRedisExtensions.Get<List<MyBagSerial>>(db, CacheKey.SerialKey);
                                bagkey.Add(bsent);
                                StackExchangeRedisExtensions.Set(db, CacheKey.SerialKey, bagkey);
                            }
                            else
                            {
                                List<MyBagSerial> bsList = new List<MyBagSerial>();
                                bsList.Add(bsent);
                                StackExchangeRedisExtensions.Set(db, CacheKey.SerialKey, bsList);
                            }
                            rt.InsertSerial(bsent);
                        }
                        else
                        {
                            Code = "ERROR";
                            ResponseMessage = "金豆抢完了！";
                            bag.BagStatus = 1;
                            rt.ChangeBagStatus(bag);
                            var setcache = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                            setcache.Remove(bagcache.Where(x => x.RID == outbag.RID).FirstOrDefault());
                            if (setcache.Count > 0)
                            {
                                setcache.Add(outbag);
                            }
                            StackExchangeRedisExtensions.Remove(db, CacheKey.BagKey);
                            StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, setcache);
                        }
                    }
                }
                else
                {
                    StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bag);
                }
            }
            catch (Exception ex)
            {

                StackExchangeRedisExtensions.Set(db, "msg", ex.Message);
            }

            return Ok(new APIResponse<RBCreateBag>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = Result
            });
        }
        [Route("seruserbean")]
        [HttpPost]
        public IHttpActionResult SetUserBean(WXUserInfo info)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口

            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            WXUserInfo Result = null;
            rt.SetUserBean(info);
            return Ok(new APIResponse<WXUserInfo>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = Result
            });
        }
        [Route("insertbag")]
        [HttpPost]
        public IHttpActionResult InsertBag(RBCreateBag bag)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
            bag.CreateTime = DateTime.Now;
            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            RBCreateBag Result = null;
            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.BagKey))
            {
                lock (_locker)
                {
                    var bagcache = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                    bagcache.Add(bag);
                    StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bagcache);
                   
                    var res = rt.InsertBag(bag);
                    Code = "SCCESS";
                    ResponseMessage = "金豆发放成功！";
                }
            }
            else
            {
                List<RBCreateBag> crbag = new List<RBCreateBag>();
                crbag.Add(bag);
                StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, crbag);
                Code = "SCCESS";
                ResponseMessage = "金豆发放成功！";
                var res = rt.InsertBag(bag);
            }
            return Ok(new APIResponse<RBCreateBag>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = Result
            });
        }
        [Route("inserttext")]
        [HttpPost]
        public IHttpActionResult InsertText(MessageRecord record)
        {
            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            MessageRecord Result = null;
            WXUserInfo user = new WXUserInfo();
            try
            {
                var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
                record.CreateTime = DateTime.Now;
                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.MsgRecord))
                {
                    var bagcache = StackExchangeRedisExtensions.Get<List<MessageRecord>>(db, CacheKey.MsgRecord);
                    if (record.MType == 1)
                    {
                        record.BagID = record.BagID.ToString();
                        rt.InsertMessageRecordByBag(record);
                        bagcache.Add(record);
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, bagcache, 240);
                        Code = "SCCESS";
                        ResponseMessage = "保存聊天记录成功！";
                    }
                    else if (record.MType == 0)
                    {

                        bagcache.Add(record);
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, bagcache, 240);
                        var res = rt.InsertMessageRecordByText(record);
                        Code = "SCCESS";
                        ResponseMessage = "保存聊天记录成功！";
                    }
                    else
                    {
                        bagcache.Add(record);
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, bagcache, 240);
                        var res = rt.InsertMessageRecordByImg(record);
                        Code = "SCCESS";
                        ResponseMessage = "保存聊天记录成功！";
                    }
                }
                else
                {
                    if (record.MType == 1)
                    {
                        List<MessageRecord> rec = new List<MessageRecord>();
                        record.BagID = record.BagID.ToString();
                        rec.Add(record);
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, rec);
                        var res = rt.InsertMessageRecordByBag(record);
                        Code = "SCCESS";
                        ResponseMessage = "保存聊天记录成功！";
                    }
                    else if (record.MType == 0)
                    {
                        List<MessageRecord> rec = new List<MessageRecord>();
                        record.BagID = record.BagID.ToString();
                        rec.Add(record);
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, rec);
                        var res = rt.InsertMessageRecordByText(record);
                        Code = "SCCESS";
                        ResponseMessage = "保存聊天记录成功！";
                    }
                    else
                    {
                        List<MessageRecord> rec = new List<MessageRecord>();
                        record.BagID = record.BagID.ToString();
                        rec.Add(record);
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, rec);
                        var res = rt.InsertMessageRecordByImg(record);
                        Code = "SCCESS";
                        ResponseMessage = "保存聊天记录成功！";
                    }
                }
                user = rt.GetUserInfo(new WXUserInfo() { openid = record.UserID }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                StackExchangeRedisExtensions.Set(db, "msg", ex.Message, 240);
            }

            return Ok(new APIResponse<WXUserInfo>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = user
            });
        }
        [Route("getmsglist")]
        [HttpGet]
        public IHttpActionResult GetMsgList(string time, int pageIndex = 1)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
            var curTime = time == null ? DateTime.Now.ToString() : Convert.ToDateTime(time).AddHours(-2).ToString();
            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            List<MessageRecord> Result = null;
            try
            {
                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.MsgRecord))
                {
                    DateTime dt = Convert.ToDateTime(curTime);
                    var bagcache = StackExchangeRedisExtensions.Get<List<MessageRecord>>(db, CacheKey.MsgRecord).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    Code = "SCCESS";
                    ResponseMessage = "获取聊天记录成功！";
                    Result = bagcache;
                }
                else
                {
                    DateTime startdt = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 0:00:00");
                    DateTime enddt = Convert.ToDateTime(DateTime.Now.ToShortDateString() + " 23:59:59");
                    var msgList = rt.GetMsgList(startdt, enddt);
                    if (msgList != null)
                    {
                        StackExchangeRedisExtensions.Set(db, CacheKey.MsgRecord, msgList);
                        Result = msgList.Skip(pageSize * (pageIndex - 1)).OrderByDescending(x => x.CreateTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
                    }
                    Code = "SCCESS";
                    ResponseMessage = "获取聊天记录成功！";
                }
            }
            catch (Exception ex)
            {
                StackExchangeRedisExtensions.Set(db, "msg", ex.Message, 120);
            }

            return Ok(new APIResponse<List<MessageRecord>>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = Result
            });
        }
        [Route("getbaglist")]
        [HttpGet]
        public IHttpActionResult GetBagList(string bagId)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口

            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            List<MyBagSerial> result = new List<MyBagSerial>();
            Guid gRID = Guid.Parse(bagId);
            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.SerialKey))
            {
                var bagcache = StackExchangeRedisExtensions.Get<List<MyBagSerial>>(db, CacheKey.SerialKey).Where(x => x.RID == gRID).ToList();
                result = bagcache;
                if (bagcache.Count > 0)
                {
                    Code = "SCCESS";
                    ResponseMessage = "获取已抢金豆列表成功！";
                }
                else
                {
                    Code = "ERROR";
                    ResponseMessage = "获取已抢金豆列表失败！";
                }
            }
            else
            {
                rt.GetUserSerialList(new MyBagSerial() { RID = gRID });
            }
            return Ok(new APIResponse<List<MyBagSerial>>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = result
            });
        }

        [Route("getbagcount")]
        [HttpGet]
        public IHttpActionResult GetRedBagCount(string bagId)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口

            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            RBCreateBag result = new RBCreateBag();
            Guid gRID = Guid.Parse(bagId);
            var ret = rt.GetSendBag(new RBCreateBag() { RID = Guid.Parse(bagId) }).FirstOrDefault();
            Code = "SCCESS";
            ResponseMessage = "获取已抢金豆列表成功！";
            return Ok(new APIResponse<RBCreateBag>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = ret
            });
        }
        [Route("getHasBag")]
        [HttpGet]
        public IHttpActionResult GetHasBag(string bagId, string userId)
        {
            try
            {
                var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口

                var Code = string.Empty;
                var ResponseMessage = string.Empty;
                RBCreateBag result = new RBCreateBag();
                Guid gRID = Guid.Parse(bagId.ToUpper());

                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.SerialKey))
                {
                    var ret = StackExchangeRedisExtensions.Get<List<RBBagSerial>>(db, CacheKey.SerialKey).Where(x => x.RID == gRID).ToList();
                    if (StackExchangeRedisExtensions.HasKey(db, CacheKey.WxUserList))
                    {
                        var userList = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList);
                        if (userList == null)
                        {
                            userList = rt.GetUserInfo(new WXUserInfo() { openid = userId });
                        }
                        foreach (var item in userList)
                        {
                            item.nickname = userList.ToList().Where(x => x.openid == item.openid).FirstOrDefault().nickname;
                        }
                        //var userBag= rt.GetUserSerial(new MyBagSerial() { RID = gRID, UserId = userId });
                        var bagkeysss = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                        var userBag = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey).Where(x => x.RID == gRID).FirstOrDefault();
                        var userGetBag = ret.Where(x => x.UserId == userId).FirstOrDefault();
                        if (userGetBag != null)
                        {
                            if (userBag.SerialList == null)
                            {
                                userBag.SerialList = new List<RBBagSerial>();
                                userBag.SerialList = ret;
                            }
                            else
                            {
                                userBag.SerialList = ret;
                            }
                            int i = 0;
                            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.WxUserList))
                            {
                                foreach (var item in userBag.SerialList)
                                {
                                    var hasUser = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList).Where(x => x.openid == item.UserId).FirstOrDefault();

                                    if (hasUser == null)
                                    {
                                        var users = rt.GetUserInfo(new WXUserInfo() { openid = item.UserId }).FirstOrDefault();
                                        item.nickname = users.nickname;
                                        item.UserId = users.openid;
                                        StackExchangeRedisExtensions.Set(db, CacheKey.WxUserList, users);
                                    }
                                    else
                                    {
                                        item.nickname = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList).Where(x => x.openid == item.UserId).FirstOrDefault().nickname;
                                    }
                                }
                            }

                            if (userGetBag != null)
                            {
                                result = userBag;
                                Code = "ERROR";
                                ResponseMessage = "用户已抢到该红包！";
                            }
                            else
                            {
                                Code = "SCCESS";
                                ResponseMessage = "用户尚未抢到该红包！";
                                result = userBag;
                            }
                        }
                        else
                        {
                            Code = "SCCESS";
                            ResponseMessage = "用户尚未抢到该红包！";
                            result = userBag;
                        }
                    }
                    else
                    {
                        var user = rt.GetUserInfo(new WXUserInfo() { openid = userId });
                        if (user != null)
                        {
                            foreach (var item in user)
                            {
                                item.nickname = user.ToList().Where(x => x.openid == item.openid).FirstOrDefault().nickname;
                            }
                            var userBag = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey).Where(x => x.RID == gRID && x.UserId == userId).FirstOrDefault();
                            userBag.SerialList = new List<RBBagSerial>();
                            userBag.SerialList = ret;
                            var serial = rt.GetUserSerialList(new MyBagSerial() { RID = gRID });
                            if (serial.Count != 0)
                            {
                                result = userBag;
                                Code = "ERROR";
                                ResponseMessage = "用户已抢到该红包！";
                            }
                            else
                            {
                                Code = "SCCESS";
                                ResponseMessage = "用户尚未抢到该红包！";
                                result = userBag;
                            }
                        }
                    }
                }
                else
                {
                    Code = "SCCESS";
                    ResponseMessage = "用户尚未抢到该红包！";
                }
                return Ok(new APIResponse<RBCreateBag>
                {
                    Code = Code,
                    ResponseMessage = ResponseMessage,
                    Result = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<RBCreateBag>
                {
                    Code = "Error",
                    ResponseMessage = ex.Message
                });
            }

        }
        RBCreateBag GenerateBag(RBCreateBag bag, out RBCreateBag outbag, out decimal curAmount)
        {
            bag.BagNum -= 1;
            curAmount = 0;
            if (bag.BagNum != 0)
            {
                Random ran = new Random();
                var Num = Decimal.ToInt32(bag.BagAmount * 100 - bag.BagNum);
                double RandKey = ran.Next(1, Num);
                decimal f = (decimal)(RandKey * 0.01);
                bag.BagAmount -= f;
                curAmount = f;
            }
            else
            {
                curAmount = bag.BagAmount;
                bag.BagAmount = 0;
            }
            outbag = bag;
            return outbag;
        }



        [Route("wxGetUserInfo")]
        [HttpGet]
        public IHttpActionResult wxGetUserInfo(string url, string code)
        {
            var Code = string.Empty;
            var ResponseMessage = string.Empty;
            getAccessTokenUrl = url;
            Code = "SCCESS";
            string accessToken = string.Empty;
            string openid = string.Empty;
            WXUserInfo UserInfo = new WXUserInfo();
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
            try
            {
                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.WxAccessToken))
                {
                    var wxToken = StackExchangeRedisExtensions.Get<string>(db, CacheKey.WxAccessToken);
                    var urls = string.Format(GetOpenID, AppId, AppSecret, code);
                    var msg = CallBackUrl(urls);//获取用户OpenId
                                                //获取用户信息
                    var dep = JsonConvert.DeserializeObject<AccessTokenOpenId>(msg);
                    //if (dep.refresh_token != "")
                    //{
                    //    StackExchangeRedisExtensions.Set(db, CacheKey.WxAccessToken, dep.refresh_token);
                    //}
                    var token = StackExchangeRedisExtensions.Get(db, CacheKey.WxAccessToken);
                    var UserInfoMsg = CallBackUrl(string.Format(GetUser, token, dep.openid));
                    UserInfo = JsonConvert.DeserializeObject<WXUserInfo>(UserInfoMsg);
                    if (!StackExchangeRedisExtensions.HasKey(db, CacheKey.WxUserList))
                    {
                        var wxUserList = new List<WXUserInfo>();

                        wxUserList.Add(UserInfo);
                        StackExchangeRedisExtensions.Set(db, CacheKey.WxUserList, wxUserList);
                        var userMsg = rt.GetUserInfo(UserInfo);
                        if (userMsg == null)
                        {
                            UserInfo.hasImg = 0;
                            rt.InsertUserInfo(UserInfo);
                        }
                        else
                        {
                            UserInfo.hasImg = userMsg.FirstOrDefault().hasImg;
                        }
                    }
                    else
                    {
                        var wxUserLists = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList);
                        var wxUserExists = wxUserLists.Where(x => x.openid == UserInfo.openid).FirstOrDefault();
                        if (wxUserExists == null)
                        {
                            UserInfo.hasImg = 0;
                            wxUserLists.Add(UserInfo);
                            StackExchangeRedisExtensions.Set(db, CacheKey.WxUserList, wxUserLists);
                            var userMsg = rt.GetUserInfo(UserInfo);
                            if (userMsg == null)
                            {
                                UserInfo.hasImg = 0;
                                rt.InsertUserInfo(UserInfo);
                            }
                            else
                            {
                                UserInfo.hasImg = userMsg.FirstOrDefault().hasImg;
                            }
                        }
                    }
                    wxCurrentUser = UserInfo;
                    StackExchangeRedisExtensions.Set(db, "wxCurrentUser", wxCurrentUser);
                }
                else
                {
                    GetAccessToken("", out accessToken, out openid);
                    StackExchangeRedisExtensions.Set(db, CacheKey.WxAccessToken, accessToken, 120);
                    var urls = string.Format(GetOpenID, AppId, AppSecret, code);
                    var openIdmsg = CallBackUrl(urls);//获取用户OpenId
                                                      //获取用户信息
                    var dep = JsonConvert.DeserializeObject<WXUserInfo>(openIdmsg);
                    var UserInfoMsg = CallBackUrl(string.Format(GetUser, accessToken, dep.openid));
                    UserInfo = JsonConvert.DeserializeObject<WXUserInfo>(UserInfoMsg);

                    if (!StackExchangeRedisExtensions.HasKey(db, CacheKey.WxUserList))
                    {
                        var wxUserList = new List<WXUserInfo>();
                        var userMsg = rt.GetUserInfo(UserInfo);
                        if (userMsg == null)
                        {
                            UserInfo.hasImg = 0;
                            rt.InsertUserInfo(UserInfo);
                        }
                        else
                        {
                            UserInfo.hasImg = userMsg.FirstOrDefault().hasImg;
                        }
                        wxUserList.Add(UserInfo);
                        StackExchangeRedisExtensions.Set(db, CacheKey.WxUserList, wxUserList);
                    }
                    else
                    {
                        var wxUserLists = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList);
                        var wxUserExists = wxUserLists.Where(x => x.openid == UserInfo.openid).FirstOrDefault();
                        if (wxUserExists == null)
                        {
                            var userMsg = rt.GetUserInfo(UserInfo);
                            if (userMsg == null)
                            {
                                UserInfo.hasImg = 0;
                                rt.InsertUserInfo(UserInfo);
                            }
                            else
                            {
                                UserInfo.hasImg = userMsg.FirstOrDefault().hasImg;
                            }
                            wxUserLists.Add(UserInfo);
                            StackExchangeRedisExtensions.Set(db, CacheKey.WxUserList, wxUserLists);
                        }
                    }
                    wxCurrentUser = UserInfo;
                    StackExchangeRedisExtensions.Set(db, "wxCurrentUser", wxCurrentUser);
                }
            }
            catch (Exception ex)
            {

                StackExchangeRedisExtensions.Set(db, "02", ex.Message);
            }

            ResponseMessage = "用户已抢到该红包！";
            return Ok(new APIResponse<WXUserInfo>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = UserInfo
            });
        }
        [Route("getAmtCode")]
        [HttpGet]
        public IHttpActionResult GetAmtCode(string bagId)
        {
            string code = string.Empty;
            string responseMessage = string.Empty;
            var info = new WXUserInfo();
            try
            {
                if (StackExchangeRedisExtensions.HasKey(db, CacheKey.WxUserList))
                {
                    var res = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList).Where(x => x.openid == bagId).FirstOrDefault();
                    if (res != null)
                    {
                        info = res;
                        code = "SCCESS";
                        responseMessage = "获取用户信息成功";
                    }
                }
                else
                {
                    var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
                    var userinfoList = rt.GetUserInfo(new WXUserInfo() { openid = bagId }).FirstOrDefault();
                    info = userinfoList;
                    code = "SCCESS";
                    responseMessage = "获取用户信息成功";
                }
                return Ok(new APIResponse<WXUserInfo>
                {
                    Code = code,
                    ResponseMessage = responseMessage,
                    Result = info
                });
            }
            catch (Exception ex)
            {
                return Ok(new APIResponse<WXUserInfo>
                {
                    Code = "SCCESS",
                    ResponseMessage = "获取用户信息失败",
                    Result = null
                });
            }
        }
        [Route("uploadimg")]
        [HttpPost]
        public IHttpActionResult UploadFile(ImageModel img)
        {
            var rt = ISoftSmart.Core.IoC.IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//使用接口
            IMG_BASE64 = img.ImgFile;
            UploadImage imgs = new UploadImage();
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(IMG_BASE64);
            var imgFile = BytToImg(IMG_BASE64);
            WXUserInfo user = new WXUserInfo()
            {
                hasImg = 1,
                openid = wxCurrentUser == null ? "olDlVsy5vYjAhbWIDMYaj5PSVp04" : wxCurrentUser.openid
            };
            var res = rt.SetUserImage(user);
            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.WxUserList))
            {
                var openid = wxCurrentUser == null ? "olDlVsy5vYjAhbWIDMYaj5PSVp04" : wxCurrentUser.openid;
                var userList = StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList);
                if (userList.Count > 0)
                {
                    var userInfo = userList.Where(x => x.openid == openid).FirstOrDefault();
                    if (userInfo != null)
                    {
                        userList.Remove(userInfo);
                        userInfo.hasImg = 1;
                        userList.Add(userInfo);
                        StackExchangeRedisExtensions.Set(db, CacheKey.WxUserList, userList);
                    }
                }
            }
            else
            {
                var openid = wxCurrentUser == null ? "olDlVsy5vYjAhbWIDMYaj5PSVp04" : wxCurrentUser.openid;
                var userInfo = rt.GetUserInfo(new WXUserInfo() { openid = openid }).FirstOrDefault();
                userInfo.hasImg = 1;
                rt.SetUserImage(userInfo);
                StackExchangeRedisExtensions.Get<List<WXUserInfo>>(db, CacheKey.WxUserList);
            }
            //获取文件储存路径
            return Ok(new APIResponse<string>
            {
                Code = "SCCESS",
                ResponseMessage = "上传成功"
            });
        }
        public System.Drawing.Image BytToImg(string byt)
        {
            byte[] arr = Convert.FromBase64CharArray(byt.ToCharArray(), 0, byt.Length);
            MemoryStream ms = new MemoryStream(arr);
            try
            {
                var bmp = Image.FromStream(ms);
                var bmpfileName = wxCurrentUser == null ? "olDlVsy5vYjAhbWIDMYaj5PSVp04.jpg" : wxCurrentUser.openid + ".jpg";
                string root = HttpContext.Current.Server.MapPath("~/QRFile/");
                if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/QRFile/")))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/QRFile/"));
                }
                var path = root + bmpfileName;
                bmp.Save(path, ImageFormat.Png);

                ms.Close();
                return bmp;
            }
            catch (Exception ex)
            {
                ms.Close();
                return null;
            }
        }
        //获取微信凭证access_token的接口
        public static string getAccessTokenUrl = string.Empty;

        #region 获取微信凭证
        public void GetAccessToken(string wechat_id, out string accessToken, out string openid)
        {

            string respText = "";
            string url = getAccessTokenUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (Stream resStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                respText = reader.ReadToEnd();
                resStream.Close();
            }
            var dep = JsonConvert.DeserializeObject<AccessTokenOpenId>(respText);

            // Dictionary<string, object> respDic = (Dictionary<string, object>)Jss.DeserializeObject(respText);
            accessToken = dep.access_token;
            openid = dep.openid;
        }
        string CallBackUrl(string url)
        {
            try
            {
                string respText = "";
                getAccessTokenUrl = url;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(getAccessTokenUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (Stream resStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(resStream, Encoding.UTF8);
                    respText = reader.ReadToEnd();
                    resStream.Close();
                }
                return respText;
            }
            catch (Exception ex)
            {
                //l.WriteLogFile(ex.Message);
                //l.WriteLogFile(ex.StackTrace);
                return null;
            }

        }

        #endregion 获取微信凭证

    }
}

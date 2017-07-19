using ISoftSmart.API.App_Start;
using ISoftSmart.Core.IoC;
using ISoftSmart.Core.RedisClient;
using ISoftSmart.Inteface.Implements;
using ISoftSmart.Inteface.Inteface;
using ISoftSmart.Model;
using ISoftSmart.Model.AD;
using ISoftSmart.Model.AD.My;
using ISoftSmart.Model.RB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace ISoftSmart.API.Controllers
{
    [RoutePrefix("api/test")]
    public class HomeController : BaseController, IRequiresSessionState
    {
        private static object _locker = new object();
        [Route("t")]
        [HttpGet]
        public IHttpActionResult Index()
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


            var db = RedisManager.Instance.GetDatabase();
            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.BagKey))
            {
                var bagcaches = StackExchangeRedisExtensions.Get<RBCreateBag>(db, CacheKey.BagKey);
                return Ok(new APIResponse<RBCreateBag>
                {
                    Code = "SUCCESS",
                    ResponseMessage = "获取列表成功！",
                    Result = bagcaches
                });
            }
            else
            {
                var rt = IoCFactory.Instance.CurrentContainer.Resolve<IRedBag>();//注册对象
                var bag = IoCFactory.Instance.CurrentContainer.Resolve<RBCreateBag>();//注册对象
                bag.BagStatus = 0;
                bag = rt.GetBag(bag);
                if (bag != null)
                {
                    StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bag);
                    var bagcaches = StackExchangeRedisExtensions.Get<RBCreateBag>(db, CacheKey.BagKey);
                    return Ok(new APIResponse<RBCreateBag>
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
            var db = RedisManager.Instance.GetDatabase();
            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.BagKey))
            {
                lock (_locker)
                {
                    var bagcache = StackExchangeRedisExtensions.Get<RBCreateBag>(db, CacheKey.BagKey);
                    if (bagcache.BagNum > 0)
                    {
                        decimal curAmount = 0;
                        var openResult = GenerateBag(bagcache, out curAmount);
                        Code = "SUCCESS";
                        ResponseMessage = "抢到" + curAmount + "元！";
                        Result = openResult;
                        RedisQueueManager.Push<RBCreateBag>(CacheKey.OpenBagKey, bagcache);
                        StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bagcache);
                    }
                    else
                    {
                        Code = "ERROR";
                        ResponseMessage = "红包抢完了！";
                        bag.BagStatus = 1;
                        //RedisQueueManager.DoQueue<int>((s) =>
                        //{
                        //    rt.ChangeBagStatus(bag);
                        //}, CacheKey.OpenBagKey);
                        rt.ChangeBagStatus(bag);
                        StackExchangeRedisExtensions.Remove(db, CacheKey.BagKey);
                    }
                }
            }
            else
            {
                StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bag);
            }
            return Ok(new APIResponse<RBCreateBag>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = Result
            });
            //});
            //return Ok(new APIResponse<RBCreateBag>
            //{
            //    Code = Code,
            //    ResponseMessage = ResponseMessage,
            //    Result = Result
            //});
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

            bag.CreateTime = DateTime.Now;
            var db = RedisManager.Instance.GetDatabase();
            if (StackExchangeRedisExtensions.HasKey(db, CacheKey.BagKey))
            {
                lock (_locker)
                {
                    var bagcache = StackExchangeRedisExtensions.Get<List<RBCreateBag>>(db, CacheKey.BagKey);
                    bagcache.Add(bag);
                    StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bagcache, 240);
                    var res = rt.InsertBag(bag);
                    if (res > 0)
                    {
                        Code = "SCCESS";
                        ResponseMessage = "金豆发放成功！";
                    }
                    else
                    {
                        Code = "ERROR";
                        ResponseMessage = "金豆发放失败！";
                    }
                }
            }
            else
            {
                StackExchangeRedisExtensions.Set(db, CacheKey.BagKey, bag);
                var res = rt.InsertBag(bag);
                if (res > 0)
                {
                    Code = "SCCESS";
                    ResponseMessage = "金豆发放成功！";
                }
                else
                {
                    Code = "ERROR";
                    ResponseMessage = "金豆发放失败！";
                }
            }
            return Ok(new APIResponse<RBCreateBag>
            {
                Code = Code,
                ResponseMessage = ResponseMessage,
                Result = Result
            });
        }
        RBCreateBag GenerateBag(RBCreateBag bag, out decimal curAmount)
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
            return bag;
        }
        [Serializable]
        public class UserInfo
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
        }
    }
}

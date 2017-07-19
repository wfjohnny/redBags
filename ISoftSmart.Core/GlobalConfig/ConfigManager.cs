using ISoftSmart.Core.GlobalConfig.Models;
using ISoftSmart.Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.GlobalConfig
{
    /// <summary>
    /// 框架级配置信息初始化，默认使用xml进行存储
    /// </summary>
    public class ConfigManager
    {
        #region Constructors & Fields
        private static ConfigModel _config;
        private static string _fileName = Path.Combine(Directory.GetCurrentDirectory(), "ConfigConstants.json");
        private static object _lockObj = new object();

        static ConfigManager()
        {
            _init = new ConfigModel();
            _init.SSO.Domain = "http://localhost:35044";
            _init.SSO.SSOKey = "SSOKeyCert";
            _init.SSO.Provider = "Redis";
            _init.SSO.TokenKey = "TokenKey";
            _init.Caching.ExpireMinutes = 20;
            _init.Caching.Provider = "RedisCache";
            _init.Logger.Level = "DEBUG";
            _init.Logger.ProjectName = "Lind.DDD";
            _init.Logger.Type = "File";
            _init.MongoDB.DbName = "Test";
            _init.MongoDB.Host = "localhost:27017";
            _init.MongoDB.UserName = string.Empty;
            _init.MongoDB.Password = string.Empty;
            _init.Queue.FilePath = "FileQueue";
            _init.Queue.Type = "Redis";
            _init.Messaging.Email_Address = "bfyxzls@sina.com";
            _init.Messaging.Email_DisplayName = "bfyxzls";
            _init.Messaging.Email_Host = "smtp.sina.com";
            _init.Messaging.Email_Password = "123456";
            _init.Messaging.Email_Port = 21;
            _init.Messaging.Email_UserName = "仓储大叔";
            _init.Messaging.RtxApi = "http://192.168.1.8:8012/sendnotifynew.cgi?";
            _init.Messaging.SMSCharset = "utf-8";
            _init.Messaging.SMSGateway = "http://sms.yourname.com/Msg/SendMessage";
            _init.Messaging.SMSKey = "04fa25475e07669d4809d334f08fb35b";
            _init.Messaging.SMSSignType = "MD5";
            _init.Messaging.SMSItemID = 1011;
            _init.Redis.Host = "localhost:6379";
            _init.Redis.Proxy = 0;
            _init.Socket.CommandPort = 8404;
            _init.Socket.DataPort = 8403;
            _init.Socket.ServerHost = "localhost";
            _init.Pub_Sub.Interval = 100000;
            _init.Pub_Sub.RepeatNum = 10;
            _init.DomainEvent.Type = "Memory";
            _init.DomainEvent.RedisKey = "DomainEventBus";
            _init.IocContaion.IoCType = 1;
            _init.IocContaion.AoP_CacheStrategy = "EntLib";
            _init.LindMQ.AutoEmptyForDay = 1;
            _init.LindMQ.Config_QueueCount = 5;
            _init.LindMQ.LindMq_TopicKey = "Lind_MQ_Topic";
            _init.LindMQ.LindMqKey = "Lind_MQ_";
            _init.LindMQ.QueueOffsetKey = "Lind_MQ_ConsumerOffset";
            _init.LindSocket.BufferSize = 4096;
            _init.LindSocket.Host = "127.0.0.1";
            _init.LindSocket.Port = 8484;
            _init.LindSocket.ListenMaxCount = 20;


            string[] blacklist = {
                                     "System",
                                     "StackExchange",
                                     "Microsoft",
                                     "Autofac",
                                     "Quartz",
                                     "EntityFramework",
                                     "MySql",
                                     "MongoDB",
                                     "log4net",
                                     "AutoMapper",
                                     "NPOI",
                                     "CrystalQuartz",
                                     "Gma.QrCodeNet",
                                     "HtmlAgilityPack",
                                     "Common.Logging",
                                     "NetPay",
                                     "ServiceStack",
                                     "Newtonsoft.Json",
                                     "Robots",
                                     "CsQuery",
                                     "Dapper"};
            _init.AutoLoadDLL_BlackList = string.Join(",", blacklist);
            _init.Versions.Add(new Models.Version
            {
                Code = "1.0.1",
                Info = "字符串配置项"
            });
            _init.Versions.Add(new Models.Version
            {
                Code = "1.0.2",
                Info = "面向对象的配置项"
            });
            _init.Cat.CatDomain = new CatDomain("test");
            _init.Cat.CatServers = new List<CatServer> { new CatServer() };
        }
        /// <summary>
        /// 模型初始化
        /// </summary>
        private static ConfigModel _init;
        #endregion

        /// <summary>
        /// 配置字典,单例模式
        /// </summary>
        /// <returns></returns>
        public static ConfigModel Config
        {
            get
            {

                if (_config == null)
                {
                    lock (_lockObj)
                    {
                        var old = _init;// SerializationHelper.DeserializeFromJson<ConfigModel>(_fileName);

                        //var old = SerializationHelper.DeserializeFromJson<ConfigModel>(_fileName);

                        if (old != null)
                        {
                            _config = old;
                        }
                        else
                        {
                            SerializationHelper.SerializableToJson(_fileName, _init);
                            _config = _init;
                        }

                    }

                }
                return _config;

            }
        }
    }
}

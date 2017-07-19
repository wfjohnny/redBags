using ISoftSmart.Core.Domain.Entities;
using ISoftSmart.Core.GlobalConfig;
using ISoftSmart.Core.MongoDbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Logger.Implements
{
    /// <summary>
    /// 日志表
    /// </summary>
    internal class GlobalLogger : EntityStr
    {

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 日志级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 日志主要内容
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 日志详细内容
        /// </summary>
        public string Detail { get; set; }
        /// <summary>
        /// 当前用户名
        /// </summary>
        public string CurrentUserName { get; set; }
    }
    /// <summary>
    /// 使用MongoDB进行日志持久化
    /// </summary>
    internal class MongoLogger : LoggerBase
    {
        private void AddToMongo(string message, string level, string detail = null)
        {
            var entity = new GlobalLogger
            {
                Level = level,
                Message = message,
                ProjectName = ConfigManager.Config.Logger.ProjectName ?? string.Empty,
                Detail = detail
            };

            MongoManager<GlobalLogger>.Instance.InsertOne(entity);
        }
        protected override void InputLogger(string message)
        {
            AddToMongo(message, "Info");
        }
        public override void Logger_Debug(string message)
        {
            AddToMongo(message, "Debug");
        }
        public override void Logger_Error(Exception ex)
        {
            AddToMongo(ex.Message, "Error", ex.StackTrace);
        }
        public override void Logger_Fatal(string message)
        {
            AddToMongo(message, "Fatal");
        }
        public override void Logger_Info(string message)
        {
            AddToMongo(message, "Info");
        }
        public override void Logger_Warn(string message)
        {
            AddToMongo(message, "Warn");
        }
    }
}

using ISoftSmart.Core.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.RedisClient
{
    /// <summary>
    /// 对RedisCache的扩展，让它支持复杂类型、
    /// RedisValue 类型可以直接使用字节数组，因此，
    /// 调用 Get 帮助程序方法时，它会将对象序列化为字节流，然后再缓存该对象。
    /// 检索项目时，项目会重新序列化为对象，然后返回给调用程序。
    /// </summary>
    public static class StackExchangeRedisExtensions
    {
        #region Ext Methods
        /// <summary>
        /// 得到键所对应的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDatabase cache, string key)
        {

            return SerializeMemoryHelper.DeserializeFromJson<T>(cache.StringGet(key));
        }
        /// <summary>
        /// 得到键所对应的值
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(this IDatabase cache, string key)
        {
            return SerializeMemoryHelper.DeserializeFromJson<object>(cache.StringGet(key));
        }
        /// <summary>
        /// 设置键对应的值,过期时间后自己删除
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMinutes"></param>
        public static void Set(this IDatabase cache, string key, object value, int expireMinutes)
        {
            string json = SerializeMemoryHelper.SerializeToJson(value);
            cache.StringSet(key, json, TimeSpan.FromMinutes(expireMinutes));
        }
        /// <summary>
        /// 设置键对应的值
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(this IDatabase cache, string key, object value)
        {
            cache.Set(key, value, 20);
        }
        /// <summary>
        /// 判断可以是否存在
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasKey(this IDatabase cache, string key)
        {
            var db = cache.KeyExists(key);
            return db;
        }
        /// <summary>
        /// 移除键及值
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        public static void Remove(this IDatabase cache, string key)
        {
            cache.KeyDelete(key);
        }
        public static void Append(this IDatabase cache, string key, object value)
        {
            string json = SerializeMemoryHelper.SerializeToJson(value);
            cache.StringAppend(key, json);
        }
        /// <summary>
        /// 出队列JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T PopJson<T>(this IDatabase cache, string key)
        {
            return SerializeMemoryHelper.DeserializeFromJson<T>(cache.ListLeftPop(key));
        }
        /// <summary>
        /// 出队列JSON
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static void PushJson<T>(this IDatabase cache, string key, T obj)
        {
            cache.ListRightPush(key, SerializeMemoryHelper.SerializeToJson<T>(obj));
        }


        #endregion
    }

}
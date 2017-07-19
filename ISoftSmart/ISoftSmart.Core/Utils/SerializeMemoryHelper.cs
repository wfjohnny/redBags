using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Utils
{
    /// <summary>
    /// 序列化与反序列化到内存
    /// </summary>
    public class SerializeMemoryHelper
    {
        #region JSON
        /// <summary>
        /// 字符串反序列化
        /// </summary>
        /// <param name="strBase64"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string jsonStr)
        {
            return JsonDeserialize<T>(jsonStr);
        }
        /// <summary>
        /// 字符串序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToJson<T>(T obj)
        {
            return JsonSerializer<T>(obj);
        }
        #endregion

        #region JSON Method

        /// <summary>
        /// JSON序列化
        /// </summary>
        private static string JsonSerializer<T>(T t)
        {

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return Newtonsoft.Json.JsonConvert.SerializeObject(t);


        }
        /// <summary>
        /// JSON反序列化
        /// </summary>
        private static T JsonDeserialize<T>(string jsonString)
        {

            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);

        }


        #endregion
    }

}

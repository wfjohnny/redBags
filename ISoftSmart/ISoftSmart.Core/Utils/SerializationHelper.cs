using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Utils
{
    /// <summary>
    /// 序列化与反序列化到文件
    /// </summary>
    public class SerializationHelper
    {
        private static object lockObj = new object();

        #region JSON
        /// <summary>
        /// 二进制序列化到磁盘
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="obj"></param>
        public static void SerializableToJson(string fileName, object obj)
        {
            lock (lockObj)
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(SerializeMemoryHelper.SerializeToJson(obj));
                    }
                }
            }
        }
        /// <summary>
        /// 二进制反序列化从磁盘到内存对象
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeserializeFromJson<T>(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    using (StreamReader sw = new StreamReader(fs, Encoding.UTF8))
                    {
                        return SerializeMemoryHelper.DeserializeFromJson<T>(sw.ReadToEnd());
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
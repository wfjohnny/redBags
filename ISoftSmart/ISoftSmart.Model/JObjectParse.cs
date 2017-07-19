using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
    public class JObjectParse
    {
        public object _data;
        public JObjectParse(object data)
        {
            this._data = data;
        }
        /// <summary>
        /// 如果是dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetDicValue(string key)
        {
            var arr = (JArray)_data;
            foreach (JObject item in arr)
            {
                if (item["name"].ToString() == key)
                {
                    if (item["value"] == null) return null;
                    return item["value"].ToString();
                }
            }
            return null;
        }
    }

}

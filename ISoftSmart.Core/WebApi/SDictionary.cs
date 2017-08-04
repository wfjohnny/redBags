using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ISoftSmart.Core.WebApi.Util;
//using _800Tele.APIServices.Util;

namespace ISoftSmart.Core.WebApi
{
    public class SDictionary : Dictionary<string, string>
    {
        public SDictionary() { }

        public SDictionary(IDictionary<string, string> dictionary)
            : base(dictionary)
        { }

        /// <summary>
        /// 添加一个新的键值对。空键或者空值的键值对将会被忽略。
        /// </summary>
        /// <param name="key">键名称</param>
        /// <param name="value">键对应的值，目前支持：string, int, long, double, bool, DateTime类型</param>
        public void Add(string key, object value)
        {
            string strValue;

            if (value == null)
            {
                strValue = null;
            }
            else if (value is string)
            {
                strValue = (string)value;
            }
            else if (value is DateTime)
            {
                strValue = CommonUtil.FormatDateTime((DateTime)value);
            }
            else if (value is Boolean)
            {
                strValue = ((Boolean)value).ToString().ToLower();
            }
            else if (value is SObject)
            {
                strValue = JsonConvert.SerializeObject(value, CommonUtil.GetJsonConverters());
            }
            else if (value is List<SObject>)
            {
                strValue = JsonConvert.SerializeObject(value, CommonUtil.GetJsonConverters());
            }
            else if (value is Nullable<DateTime>)
            {
                Nullable<DateTime> dateTime = value as Nullable<DateTime>;
                strValue = dateTime.HasValue ? CommonUtil.FormatDateTime(dateTime.Value) : null;
            }
            else if (value is short?)
            {
                Nullable<Int16> v = value as Nullable<Int16>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Int32>)
            {
                Nullable<Int32> v = value as Nullable<Int32>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Int64>)
            {
                Nullable<Int64> v = value as Nullable<Int64>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Single>)
            {
                Nullable<Single> v = value as Nullable<Single>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Decimal>)
            {
                Nullable<Decimal> v = value as Nullable<Decimal>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Double>)
            {
                Nullable<Double> v = value as Nullable<Double>;
                strValue = v.HasValue ? v.Value.ToString() : null;
            }
            else if (value is Nullable<Boolean>)
            {
                Nullable<Boolean> v = value as Nullable<Boolean>;
                strValue = v.HasValue ? v.Value.ToString().ToLower() : null;
            }
            else if (value is List<Field>)
            {
                List<Field> v = value as List<Field>;
                strValue = ParseFieldList(v);
            }
            else if (value is Byte[])
            {
                strValue = Convert.ToBase64String((Byte[])value);
            }
            else
            {
                strValue = value.ToString();
            }

            this.Add(key, strValue);
        }

        public new void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                base.Add(key, value);
            }
        }

        private String ParseFieldList(List<Field> list)
        {
            list.Sort((Field a, Field b) => { return String.Compare(a.Key, b.Key); });
            IEnumerator<Field> enumerator = list.GetEnumerator();
            StringBuilder query = new StringBuilder("[");
            Boolean first = true;
            while (enumerator.MoveNext())
            {
                string key = enumerator.Current.Key;
                string value = enumerator.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    if (!first)
                        query.Append(",");

                    query.AppendFormat("{{\"{0}\":\"{1}\"}}", key, value);
                    first = false;
                }
            }

            query.Append("]");
            return query.ToString();
        }
    }
}

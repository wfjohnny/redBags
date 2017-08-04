using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ISoftSmart.Core.WebApi.Parser
{
    public class DateTimeConverter : JsonConverter
    {
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime)
            {
                var dateTime = (DateTime)value;
                var dateTimeStr = dateTime.ToString(DATE_TIME_FORMAT, System.Globalization.DateTimeFormatInfo.InvariantInfo); ;
                writer.WriteValue(dateTimeStr);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(Int64))
            {
                //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                var dtStart = new DateTime(1970, 1, 1);
                var lTime = (Int64)reader.Value;
                return dtStart.AddMilliseconds(lTime);
            }

            var dt = new DateTime();
            if (reader.Value == null) return null;
            if (DateTime.TryParse(reader.Value.ToString(), out dt))
            {
                return dt;
            }
            else
            {
                throw new Exception(String.Format("{0}:{1}转换成DateTime失败！", reader.Path, reader.Value));
            }
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
                return true;

            return false;
        }
    }
}


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Dapper.Helper
{
   public static class ExtendHelper
    {
        #region 扩展方法 DataSet
        /// <summary>
        /// 把dataset数据转换成json的格式
        /// </summary>
        /// <param name="ds">dataset数据集</param>
        /// <returns>json格式的字符串</returns>
        public static string GetJsonByDataset(this DataSet ds)
        {
            //if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
            //{
            //    //如果查询到的数据为空则返回标记ok:false
            //    return "{\"ok\":false}";
            //}
            StringBuilder sb = new StringBuilder();
            //sb.Append("{\"ok\":true,");
            foreach (DataTable dt in ds.Tables)
            {
                //sb.Append(string.Format("\"{0}\":[", dt.TableName));

                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("{");
                    for (int i = 0; i < dr.Table.Columns.Count; i++)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), "\\r\\n").Replace(Convert.ToString((char)10), "\\r\\n");
                    }
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                    sb.Append("},");
                }
                if(dt.Rows.Count>0)
                {
                    sb.Remove(sb.ToString().LastIndexOf(','), 1);
                }
                //sb.Append("],");
            }
            //sb.Remove(sb.ToString().LastIndexOf(','), 1);
            //sb.Append("}");
            return sb.ToString();
        }
      
        /// <summary>
        /// 将object转换成为string
        /// </summary>
        /// <param name="ob">obj对象</param>
        /// <returns></returns>
        public static string ObjToStr(object ob)
        {
            if (ob == null)
            {
                return string.Empty;
            }
            else
                return ob.ToString();
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(this string jsonString)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion
    }
}

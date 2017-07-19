using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
    public class DataTablesData<T>
    {
        /// <summary>
        /// jQuery.DataTables 插件 必须要返回的
        /// </summary>
        public string sEcho { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int iTotalRecords { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int iTotalDisplayRecords { get; set; }
        /// <summary>
        /// 返回列表
        /// </summary>
        public IList<T> aaData { get; set; }
    }
}

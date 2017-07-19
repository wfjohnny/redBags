using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
    /// <summary>
    /// 分页查询数据结构
    /// </summary>
    /// <typeparam name="T">Model</typeparam>
    [Serializable]
    public class PageData<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalSize { get; set; }
        public IList<T> PageDatas { get; set; }
    }
}
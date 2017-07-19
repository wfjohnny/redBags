using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Domain.Entities
{
    /// <summary>
    /// 实体状态
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hidden = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Deleted = 3,
        /// <summary>
        /// 冻结
        /// </summary>
        Freeze = 4,
    }
}

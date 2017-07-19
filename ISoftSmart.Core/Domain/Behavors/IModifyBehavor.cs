using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Domain.Behavors
{
    /// <summary>
    /// 实体－修改行为
    /// 修改数据的行为
    /// 实体实现了接口后，可以在数据提交时自动更新这两个属性
    /// </summary>
    public interface IModifyBehavor
    {
        /// <summary>
        /// 最后更新时间
        /// </summary>
        DateTime LastModifyTime { get; set; }
        /// <summary>
        /// 最后操作人ID
        /// </summary>
        int LastModifyUserId { get; set; }
        /// <summary>
        /// 最后操作人名称
        /// </summary>
        string LastModifyUserName { get; set; }
    }
}

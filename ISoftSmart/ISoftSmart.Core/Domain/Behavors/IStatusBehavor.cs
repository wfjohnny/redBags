using ISoftSmart.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Domain.Behavors
{
    /// <summary>
    /// 实体－状态行为
    /// 实体有状态属性，可以实现这个接口
    /// </summary>
    public interface IStatusBehavor
    {
        /// <summary>
        /// 实体状态
        /// </summary>
        Status DataStatus { get; set; }
    }
}


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Domain.Behavors
{
    /// <summary>
    /// 实体－ 排序行为
    /// </summary>
    public interface ISortBehavor
    {
        /// <summary>
        /// 序列
        /// </summary>
        int SortNumber { get; set; }
    }
}

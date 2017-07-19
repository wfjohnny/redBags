using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.Domain.Entities
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public interface IEntity
    {
        DateTime AddTime { get; set; }
        DateTime LastedTime { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.AD.My
{
    /// <summary>
    /// AdUser 
    /// </summary>
    [Serializable]
    public class MyAdUser : AdUser
    {
        /// <summary>
        /// 是否显示,默认为true
        /// </summary>
        public bool IsShow = true;

        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
}

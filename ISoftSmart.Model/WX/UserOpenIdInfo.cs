using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.WX
{
    /// <summary>
    /// 获取用户Openid
    /// </summary>
   public class UserOpenIdInfo
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
}

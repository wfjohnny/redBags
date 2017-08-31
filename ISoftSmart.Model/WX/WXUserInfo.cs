using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.WX
{
    public class WXUserInfo
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string privilege { get; set; }
        public string unionid { get; set; }
        public decimal beannum { get; set; }
        public int hasImg { get; set; }
        public int Invite { get; set; }
    }
}

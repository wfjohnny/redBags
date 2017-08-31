using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.WX
{
   public class WxShareModel
    {
        public string appid { get; set; }
        public string title { get; set; }
        public string desc { get; set; }
        public string link { get; set; }
        public string imgUrl { get; set; }
        public string type { get; set; }
        public string dataUrl { get; set; }


        public string timestamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }

        public string jsapi_ticket { get; set; }
    }
}

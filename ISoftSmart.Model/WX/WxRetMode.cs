using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.WX
{
    public class WxRetMode
    {
        public string errcode { get; set;}
        public string errmsg { get; set; }
        public string ticket { get; set; }
        public int expires_in { get; set; }

        public string code { get; set; }
        public int state { get; set; }
        //{"errcode":0,"errmsg":"ok","ticket":"sM4AOVdWfPE4DxkXGEs8VJaB9d1omgNg3j_V7vF8mpJAxnMn1a3c9LgkiGqRZWvTgFYmZVEDW9qVDFmxLMgkEg","expires_in":7200}
    }
}

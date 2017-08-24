using ISoftSmart.Model.RB.My;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ISoftSmart.Model.RB
{
    public class RBCreateBag :MyRBCreateBag
    {
        public Guid RID { get; set; }
        public string UserId { get; set; }
        public decimal BagAmount { get; set; }
        public int BagNum { get; set; }
        public DateTime CreateTime { get; set; }
        public int BagStatus { get; set; }
        public Guid Winner { get; set; }
        public decimal WinnerAmount { get; set; }
        public List<RBBagSerial> SerialList { get; set; }
        public string Remark { get; set; }
        //[JsonIgnore]
        public string CurrentUserImgUrl { get; set; }
        //[JsonIgnore]
        public string nickname { get; set; }
    }
}

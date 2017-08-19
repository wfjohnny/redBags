using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.RB
{
   public class RBBagSerial
    {
        public Guid SerialId { get; set; }
        public Guid RID { get; set; }
        public string UserId { get; set; }
        public decimal BagAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public int BagNum { get; set; }
        public string nickname { get; set; }
        public string headImg { get; set; }
    }

}

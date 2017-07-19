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
        public Guid UserId { get; set; }
        public decimal BagAmount { get; set; }
        public DateTime CreateTime { get; set; }
    }

}

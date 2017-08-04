using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.BS
{
    public class BagSerial
    {
        public Guid SerialId { get; set; }
        public Guid RID { get; set; }
        public string UserId { get; set; }
        public decimal BagAmount { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

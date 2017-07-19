using ISoftSmart.Model.RB.My;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.RB
{
    public class RBCreateBag :MyRBCreateBag
    {
        public Guid RID { get; set; }
        public Guid UserId { get; set; }
        public decimal BagAmount { get; set; }
        public int BagNum { get; set; }
        public DateTime CreateTime { get; set; }
        public int BagStatus { get; set; }
        public Guid Winner { get; set; }
        public decimal WinnerAmount { get; set; }
    }
}

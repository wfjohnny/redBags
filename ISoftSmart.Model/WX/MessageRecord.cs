using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.WX
{
   public class MessageRecord
    {
        public Guid MID { get; set; }
        public string UserID { get; set; }
        public string MContent { get; set; }
        public int MType { get; set; }
        public string BagID { get; set; }
        public string BagUserID { get; set; }
        public string BagRemark { get; set; }
        public DateTime CreateTime { get; set; }
        public string HeadImgUrl { get; set; }
        public string AmtUserImg { get; set; }
        public string AmtUserID { get; set; }
        public string ImgUrl { get; set; }
        public string ImgUserID { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
   public class PageModel
    {
       public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public object Model { get; set; }
    }
}

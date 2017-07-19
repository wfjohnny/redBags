using ISoftSmart.Model.RB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Inteface.Inteface
{
    public interface IRedBag :IDependency
    {
        RBCreateBag GetBag(RBCreateBag bag);
        int ChangeBagStatus(RBCreateBag bag);
        int InsertBag(RBCreateBag bag);
    }
}

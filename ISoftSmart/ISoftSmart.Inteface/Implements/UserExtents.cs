using ISoftSmart.Inteface.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISoftSmart.Dapper.Helper;

namespace ISoftSmart.Inteface.Implements
{
    public class UserExtents : ITestUsers
    {
        void ITestUsers.Test()
        {
           
        }

        int ITestUsers.Test1()
        {
            Dapper.DapperRepository<List<Model.AD.AdUser>> ad = new Dapper.DapperRepository<List<Model.AD.AdUser>>();
            var test=ad.GetModel().ToList();
            return 111;
        }

        string ITestUsers.Test2()
        {
            Dapper.DapperRepository<List<Model.AD.AdUser>> ad = new Dapper.DapperRepository<List<Model.AD.AdUser>>();
            
          var t=  Dapper.Helper.DbHelperMySQL.Query("select * from t_account where id=4");
            return t;
        }
    }
}

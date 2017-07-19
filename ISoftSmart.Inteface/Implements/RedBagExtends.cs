using ISoftSmart.Inteface.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISoftSmart.Model.RB;
using ISoftSmart.Dapper.Helper;
using System.Data;
using System.Data.SqlClient;

namespace ISoftSmart.Inteface.Implements
{
    public class RedBagExtends : IRedBag
    {
        public RedBagExtends()
        { }

        public int ChangeBagStatus(RBCreateBag bag)
        {
            SqlParameter[] sp = new SqlParameter[]
           {
                new SqlParameter("@BagStatus",bag.BagStatus),
                new SqlParameter("@RID",bag.RID)
           };
            var result = Dapper.Helper.SQLHelper.Execute("update CreateBag set BagStatus=@BagStatus where RID=@RID", sp, CommandType.Text);
            return result;
        }

        public RBCreateBag GetBag(RBCreateBag bag)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@BagStatus",bag.BagStatus)
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet("select * from CreateBag where BagStatus=@BagStatus", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<RBCreateBag>();
        }

        public int InsertBag(RBCreateBag bag)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@RID",bag.RID),
                 new SqlParameter("@UserId",bag.UserId),
                 new SqlParameter("@BagAmount",bag.BagAmount),
                 new SqlParameter("@BagNum",bag.BagNum),
                 new SqlParameter("@CreateTime",bag.CreateTime),
                 new SqlParameter("@BagStatus",bag.BagStatus),
                 new SqlParameter("@Winner",bag.Winner),
                 new SqlParameter("@WinnerAmount",bag.WinnerAmount)
             };
            var result = Dapper.Helper.SQLHelper.QueryDataSet(@"INSERT INTO CreateBag
                   ([RID]
                   ,[UserId]
                   ,[BagAmount]
                   ,[BagNum]
                   ,[CreateTime]
                   ,[BagStatus]
                   ,[Winner]
                   ,[WinnerAmount])
             VALUES
                   (@RID
                   ,@UserId
                   ,@BagAmount
                   ,@BagNum
                   ,@CreateTime
                   ,@BagStatus
                   ,@Winner
                   ,@WinnerAmount)", sp, CommandType.Text);
            if (result == "")
                return 0;
            return 1;
        }
    }
}

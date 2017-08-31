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
using ISoftSmart.Model.UserInfo;
using ISoftSmart.Model.BS.My;
using ISoftSmart.Model.WX;

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

        public List<RBCreateBag> GetBag(RBCreateBag bag)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@BagStatus",bag.BagStatus),
                new SqlParameter("@RID",bag.RID.ToString().ToUpper()),
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet("select * from CreateBag where BagStatus=@BagStatus and RID=@RID", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<List<RBCreateBag>>();
        }
        public List<RBCreateBag> GetBagInfo(RBCreateBag bag)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@RID",bag.RID.ToString().ToUpper()),
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet("select * from CreateBag where RID=@RID", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<List<RBCreateBag>>();
        }
        public List<RBCreateBag> GetSendBag(RBCreateBag bag)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@RID",bag.RID),
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet("select * from CreateBag where  RID=@RID", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<List<RBCreateBag>>();
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<WXUserInfo> GetUserInfo(WXUserInfo user)
        {
            if (user.openid == null)
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                new SqlParameter("@OpenId"," 1=1 ")
                };
                var result = Dapper.Helper.SQLHelper.QueryDataSet(@"select * from [dbo].[UserInfo] where 1=1", sp, CommandType.Text);
                if (result == "")
                    return null;
                return result.JsonDeserialize<List<WXUserInfo>>();
            }
            else
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                new SqlParameter("@OpenId",user.openid)
                };
                var result = Dapper.Helper.SQLHelper.QueryDataSet(@"select * from [dbo].[UserInfo] b where b.OpenId=@OpenId ", sp, CommandType.Text);
                if (result == "")
                    return null;
                return result.JsonDeserialize<List<WXUserInfo>>();
            }

        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<WXUserInfo> GetUserInfoByPage(WXUserInfo user, int pageindex, int pagesize,out int pageCount)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            List<SqlParameter> spList = new List<SqlParameter>();
            string strWhere = string.Empty;
            string countsql = "select * from UserInfo where 1=1";
            string sql = @"SELECT TOP " + pagesize + @" * FROM
                (
                    SELECT ROW_NUMBER() OVER (ORDER BY openid) AS RowNumber,* FROM [UserInfo]
                )   as A  
            WHERE RowNumber > " + pagesize + @"*(" + pageindex + @"-1) ";
            if (user.nickname != null)
            {
                spList.Add(new SqlParameter("@NickName", user.nickname));
                strWhere += " and NickName like '%" + user.nickname + "%' ";
            }
            spList.Add(new SqlParameter("@Invite", user.Invite));
            if (user.Invite!= -1)
            {
                strWhere += " and Invite = " + user.Invite + " ";
            }
          
            
            SqlParameter[] sp = spList.ToArray();
            SqlParameter[] spcount = spList.ToArray();
            var result = Dapper.Helper.SQLHelper.QueryDataSet(sql+strWhere, sp, CommandType.Text);
            var countresult = Dapper.Helper.SQLHelper.QueryDataSet(countsql+strWhere, spcount, CommandType.Text);
            if (countresult == "")
                pageCount = 0;
            else
            {
                pageCount = countresult.JsonDeserialize<List<WXUserInfo>>().Count;
            }
               
            if (result == "")
            {
                return null;
            }
            return result.JsonDeserialize<List<WXUserInfo>>();


        }
        public int ChangeUserStatus(WXUserInfo bag)
        {
            SqlParameter[] sp = new SqlParameter[]
           {
                new SqlParameter("@OpenId",bag.openid),
                new SqlParameter("@Invite",bag.Invite)
           };
            var result = Dapper.Helper.SQLHelper.Execute(@"
            UPDATE[dbo].[UserInfo]
               SET[Invite] =@Invite
             WHERE  [OpenId] =@OpenId", sp, CommandType.Text);
            return result;
        }
        public int InsertUserInfo(WXUserInfo user)
        {
            SqlParameter[] sp = new SqlParameter[]
              {
                 new SqlParameter("@UserId",Guid.NewGuid()),
                 new SqlParameter("@OpenId",user.openid),
                 new SqlParameter("@NickName",user.nickname),
                 new SqlParameter("@HeadImgUrl",user.headimgurl),
                 new SqlParameter("@Country",user.country),
                 new SqlParameter("@Province",user.province),
                 new SqlParameter("@City",user.city),
                 new SqlParameter("@Sex",user.sex),
                 new SqlParameter("@HasImg",user.hasImg),
                 new SqlParameter("@Invite",user.Invite),
              };
            var result = Dapper.Helper.SQLHelper.Execute(@"INSERT INTO [dbo].[UserInfo]
           ([UserId]
           ,[OpenId]
           ,[NickName]
           ,[HeadImgUrl]
           ,[Country]
           ,[Province]
           ,[City]
           ,[Sex]
           ,[HasImg]
            ,Invite)
     VALUES
           (@UserId
           ,@OpenId
           ,@NickName
           ,@HeadImgUrl
           ,@Country
           ,@Province
           ,@City
           ,@Sex
           ,@HasImg
            ,@Invite)", sp, CommandType.Text);
            return result;
        }
        public List<MyBagSerial> GetUserSerialList(MyBagSerial my)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@RID",my.RID)
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet(@"select * from [dbo].[BagSerial] b left join [dbo].[UserInfo] u
                on b.UserId = u.UserId
                where b.RID =@RID ", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<List<MyBagSerial>>();
        }
        public List<MyBagSerial> GetUserSerial(MyBagSerial my)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@RID",my.RID),
                new SqlParameter("@UserId",my.UserId)
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet(@"select * from [dbo].[BagSerial] b left join [dbo].[UserInfo] u
                on b.UserId = u.UserId
                where b.RID =@RID and u.UserId=@UserId ", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<List<MyBagSerial>>();
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
                 new SqlParameter("@WinnerAmount",bag.WinnerAmount),
                 new SqlParameter("@Remark",bag.Remark),
                 new SqlParameter("@UserImgUrl",bag.CurrentUserImgUrl),
                 new SqlParameter("@nikename",bag.nickname),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"INSERT INTO CreateBag
                   ([RID]
                   ,[UserId]
                   ,[BagAmount]
                   ,[BagNum]
                   ,[CreateTime]
                   ,[BagStatus]
                   ,[Winner]
                   ,[WinnerAmount]
                   ,[Remark]
                   ,[UserImgUrl]
                   ,[nikename])
             VALUES
                   (@RID
                   ,@UserId
                   ,@BagAmount
                   ,@BagNum
                   ,@CreateTime
                   ,@BagStatus
                   ,@Winner
                   ,@WinnerAmount,@Remark,@UserImgUrl,@nikename)", sp, CommandType.Text);
            return result;
        }

        public int InsertSerial(MyBagSerial my)
        {
            SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@SerialId",Guid.NewGuid()),
                    new SqlParameter("@UserId",my.UserId),
                    new SqlParameter("@BagAmount",my.BagAmount),
                    new SqlParameter("@CreateTime",my.CreateTime),
                    new SqlParameter("@RID",my.RID)
                };
            var result = @"INSERT INTO [dbo].[BagSerial]
               ([SerialId]
               ,[RID]
               ,[UserId]
               ,[BagAmount]
               ,[CreateTime])
         VALUES
               (@SerialId
               ,@RID
               ,@UserId
               ,@BagAmount
               ,@CreateTime)";
            var r = Dapper.Helper.SQLHelper.Execute(result, sp, CommandType.Text);
            return r;
        }

        public int InsertSerialList(List<MyBagSerial> my)
        {
            int ret = 0;
            foreach (var item in my)
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@SerialId",item.RID),
                    new SqlParameter("@UserId",item.RID),
                    new SqlParameter("@BagAmount",item.RID),
                    new SqlParameter("@CreateTime",item.RID),
                    new SqlParameter("@RID",item.RID)
                };
                var result = @"INSERT INTO [dbo].[BagSerial]
               ([SerialId]
               ,[RID]
               ,[UserId]
               ,[BagAmount]
               ,[CreateTime])
         VALUES
               (@SerialId
               ,@RID
               ,@UserId
               ,@BagAmount
               ,@CreateTime)";
                var r = Dapper.Helper.SQLHelper.Execute(result, sp, CommandType.Text);
                ret += r;
            }
            return ret;
        }



        public int SetUserBean(WXUserInfo bag)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@OpenId",bag.openid),
                 new SqlParameter("@BeanNum",bag.beannum),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"
            UPDATE [dbo].[UserInfo]
               SET [BeanNum] = [BeanNum]+@BeanNum
             WHERE OpenId=@OpenId", sp, CommandType.Text);
            return result;
        }
        public int SetUserImage(WXUserInfo bag)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@OpenId",bag.openid),
                 new SqlParameter("@HasImg",bag.hasImg),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"
            UPDATE [dbo].[UserInfo]
               SET [HasImg] = @HasImg
             WHERE OpenId=@OpenId", sp, CommandType.Text);
            return result;
        }
        public int InsertMessageRecordByText(MessageRecord record)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@MID",Guid.NewGuid()),
                 new SqlParameter("@UserID",record.UserID),
                 new SqlParameter("@MContent",record.MContent),
                 new SqlParameter("@MType",record.MType),
                 new SqlParameter("@CreateTime",record.CreateTime),
                 new SqlParameter("@HeadImgUrl",record.HeadImgUrl),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"INSERT INTO [dbo].[MessageRecord]
           ([MID]
           ,[UserID]
           ,[MContent]
           ,[HeadImgUrl]
           ,[MType]
           ,[CreateTime])
     VALUES
           (@MID
           ,@UserID
           ,@MContent
           ,@HeadImgUrl
           ,@MType
           ,@CreateTime)", sp, CommandType.Text);
            return result;
        }
        public int InsertMessageRecordByBag(MessageRecord record)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@MID",Guid.NewGuid()),
                 new SqlParameter("@MType",record.MType),
                 new SqlParameter("@BagID",record.BagID),
                 new SqlParameter("@BagUserID",record.BagUserID),
                 new SqlParameter("@BagRemark",record.BagRemark),
                 new SqlParameter("@HeadImgUrl",record.HeadImgUrl),
                 new SqlParameter("@CreateTime",record.CreateTime),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"INSERT INTO [dbo].[MessageRecord]
                   ([MID]
                   ,[HeadImgUrl]
                   ,[MType]
                   ,[BagID]
                   ,[BagUserID]
                   ,[BagRemark]
                   ,[CreateTime])
             VALUES
                   (@MID
                   ,@HeadImgUrl
                   ,@MType
                   ,@BagID
                   ,@BagUserID
                   ,@BagRemark
                   ,@CreateTime)", sp, CommandType.Text);
            return result;
        }
        public int InsertMessageRecordByImg(MessageRecord record)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@MID",Guid.NewGuid()),
                 new SqlParameter("@MType",record.MType),
                 new SqlParameter("@AmtUserID",record.AmtUserID),
                 new SqlParameter("@AmtUserImg",record.AmtUserImg),
                 new SqlParameter("@CreateTime",record.CreateTime),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"INSERT INTO [dbo].[MessageRecord]
                   ([MID]
                   ,[MType]
                   ,[AmtUserID]
                   ,[AmtUserImg]
                   ,[CreateTime])
             VALUES
                   (@MID
                   ,@MType
                   ,@AmtUserID
                   ,@AmtUserImg
                   ,@CreateTime)", sp, CommandType.Text);
            return result;
        }
        public int InsertMessageRecordByImgs(MessageRecord record)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                 new SqlParameter("@MID",Guid.NewGuid()),
                 new SqlParameter("@MType",record.MType),
                 new SqlParameter("@HeadImgUrl",record.HeadImgUrl),
                 new SqlParameter("@ImgUserID",record.ImgUserID),
                 new SqlParameter("@ImgUrl",record.ImgUrl),
                 new SqlParameter("@CreateTime",record.CreateTime),
             };
            var result = Dapper.Helper.SQLHelper.Execute(@"INSERT INTO [dbo].[MessageRecord]
                   ([MID]
                   ,[MType]
                    ,[HeadImgUrl]
                   ,[ImgUserID]
                   ,[ImgUrl]
                   ,[CreateTime])
             VALUES
                   (@MID
                   ,@MType
                    ,@HeadImgUrl
                   ,@ImgUserID
                   ,@ImgUrl
                   ,@CreateTime)", sp, CommandType.Text);
            return result;
        }
        public List<MessageRecord> GetMsgList(DateTime startTime, DateTime endTime)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@startTime",startTime),
                new SqlParameter("@endTime",endTime)
            };
            var result = Dapper.Helper.SQLHelper.QueryDataSet(@"SELECT *
          FROM [dbo].[MessageRecord] where CreateTime between @startTime and @endTime", sp, CommandType.Text);
            if (result == "")
                return null;
            return result.JsonDeserialize<List<MessageRecord>>();
        }
    }
}

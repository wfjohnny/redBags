using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace ISoftSmart.Dapper
{
    /// <summary>
    /// 使用Dapper实现的仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class DapperRepository<TEntity> :
        Core.IRepositories.IRepository<TEntity>
        where TEntity : class
    {
        IDbConnection conn;
        string connString =System.Configuration.ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
        string tableName;
        public DapperRepository()
        {
            conn = new SqlConnection(connString);
            tableName = typeof(TEntity).Name;

        }

        #region IRepository<TEntity> 成员

        public TEntity Find(params object[] id)
        {
            return conn.Get<TEntity>(id);
        }

        public IQueryable<TEntity> GetModel()
        {
            return conn.GetAll<TEntity>().AsQueryable();
        }

        public void SetDataContext(object db)
        {
            throw new NotImplementedException();
        }

        public void Insert(TEntity item)
        {
            conn.Insert(item);
        }

        public void Update(TEntity item)
        {
            conn.Update(item);
        }

        public void Delete(TEntity item)
        {
            conn.Delete(item);
        }

        #endregion

    }
}

using Loan.Entity.DB;
using Loan.Entity.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Core.Models;
using Loan.Core;
using PetaPoco;
using System.Data.SqlClient;

namespace Loan.Data
{
    public class UserDao : BaseDao, IList<UserQuery, Users>
    {
        public IEnumerable<Users> GetList(UserQuery query)
        {
            var list = database.Query<Users>("SELECT * FROM Users");
            return list;
        }

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-03-23
        /// </remarks>
        /// <param name="query"></param>
        /// <returns></returns>
        public Users Get(UserQuery query)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT * FROM users")
                                          .Append("WHERE UserName=@0", query.UserName)
                                          .Append("AND status=@0", ObjectExtension.TryInt(GlobalConst.Status.Normal));
            var info = database.SingleOrDefault<Users>(sql);
            return info;
        }
        #endregion

        #region 新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public int Insert(Users user)
        {
            var result = database.Insert("users", "userID", true, user);
            return result.TryInt();
        }
        #endregion

        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Page<Users> GetList(int pageSize, int pageIndex)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT * FROM users ORDER BY createTime ASC");
            return database.Page<Users>(pageIndex, pageSize, sql);
        }
        #endregion

        #region 更新用户状态
        /// <summary>
        /// 更新用户状态
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="id">userid</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public int ChangeStatus(int id, GlobalConst.Status status)
        {
            string strSql = string.Format("UPDATE users SET status=@status where branchcompanyid=@id");
            SqlParameter[] paras = new SqlParameter[] 
            { 
                new SqlParameter("@status", status.TryInt())
                , new SqlParameter("@id", id) 
            };

            return database.Update<Users>(strSql, paras);
        }
        #endregion

        #region 根据userID获取用户
        /// <summary>
        /// 根据userID获取用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-08
        /// </remarks>
        /// <param name="id">userID</param>
        /// <returns></returns>
        public Users GetUserByID(int id)
        {
            string sql = string.Format("SELECT * FROM users WHERE userID=@id");
            SqlParameter[] paras = new SqlParameter[] 
            { 
                new SqlParameter("@id", id)
            };

            return database.FirstOrDefault<Users>(sql.TryString(), paras);
        }
        #endregion

        #region 修改用户
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-08
        /// </remarks>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public int Update(Users user)
        {
            var result = database.Update("users", "userID", true, user);
            return result.TryInt();
        }
        #endregion
    }
}

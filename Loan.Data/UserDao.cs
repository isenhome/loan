using Loan.Entity.DB;
using Loan.Entity.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Core.Models;
using Loan.Core;

namespace Loan.Data
{
    public class UserDao : BaseDao, IList<UserQuery, User>
    {
        public IEnumerable<User> GetList(UserQuery query)
        {
            var list = database.Query<User>("SELECT * FROM Users");
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
        public User Get(UserQuery query)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT * FROM users")
                                          .Append("WHERE UserName=@0", query.UserName)
                                          .Append("AND status=@0", ObjectExtension.TryInt(GlobalConst.Status.Normal));
            var info = database.SingleOrDefault<User>(sql);
            return info;
        }
        #endregion

        public int Insert(User info)
        {
            var result = database.Insert(info);
            return result.TryInt();
        }
    }
}

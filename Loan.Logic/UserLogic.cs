using Common.Logging;
using Loan.Entity.DB;
using Loan.Entity.Query;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Core;
using Loan.Data;
using Loan.Core.Models;

namespace Loan.Logic
{
    public class UserLogic
    {
        ILog _logger = LogManager.GetLogger(typeof(UserLogic));

        #region 验证用户登录
        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-03023
        /// </remarks>
        /// <param name="query"></param>
        /// <returns></returns>
        public bool CheckUser(UserQuery query)
        {            
            try
            {
                var info = AOPFactory.CreateInstance<UserDao>().Get(query);
                if (info != null && info.Password == query.Password)
                {
                    return true;                    
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("登录时发生异常。登录用户名：{0}；错误信息：{1}", query.UserName, ex.Message));
                return false;
            }
        }
        #endregion
    }
}

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
using PetaPoco;

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

        #region 新增用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public int Add(Users user)
        {
            try
            {
                return AOPFactory.CreateInstance<UserDao>().Insert(user);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("新增用户发生异常.", ex.Message));
                return -200;
            }
        }
        #endregion

        #region 获取分公司列表
        /// <summary>
        /// 获取分公司列表
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04-15
        /// </remarks>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Page<Users> GetList(int pageSize, int pageIndex)
        {
            try
            {
                return AOPFactory.CreateInstance<UserDao>().GetList(pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("获取用户列表发生异常.", ex.Message));
                return null;
            }
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
            try
            {
                return AOPFactory.CreateInstance<UserDao>().ChangeStatus(id, status);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("更新用户状态发生异常.", ex.Message));
                return -200;
            }
        }
        #endregion

        #region 根据ID获取用户
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-07
        /// </remarks>
        /// <param name="id">userid</param>
        /// <returns></returns>
        public Users GetUserByID(int id)
        {
            try
            {
                return AOPFactory.CreateInstance<UserDao>().GetUserByID(id);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("根据userid获取用户发生异常.{0}", ex.Message));
                return null;
            }
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
        public int Edit(Users user)
        {
            try
            {
                return AOPFactory.CreateInstance<UserDao>().Update(user);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("修改用户发生异常.", ex.Message));
                return -200;
            }
        }
        #endregion
    }
}

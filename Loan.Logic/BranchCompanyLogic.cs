using Common.Logging;
using Loan.Core;
using Loan.Data;
using Loan.Entity.DB;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Logic
{
    public class BranchCompanyLogic
    {
        ILog _logger = LogManager.GetLogger(typeof(UserLogic));

        #region 新增分公司
        /// <summary>
        /// 新增分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04-13
        /// </remarks>
        /// <param name="branchCompany">分公司实体</param>
        /// <returns></returns>
        public int Add(BranchCompany branchCompany)
        {
            try
            {
                return AOPFactory.CreateInstance<BranchCompanyDao>().Insert(branchCompany);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("新增分公司发生异常.", ex.Message));
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
        public Page<BranchCompany> GetList(int pageSize, int pageIndex)
        {
            try
            {
                return AOPFactory.CreateInstance<BranchCompanyDao>().GetList(pageSize, pageIndex);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("获取分公司列表发生异常.", ex.Message));
                return null;
            }
        }
        #endregion

        #region 更新分公司状态
        /// <summary>
        /// 更新分公司状态
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-05
        /// </remarks>
        /// <param name="id">分公司id</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public int ChangeStatus(int id, GlobalConst.Status status)
        {
            try
            {
                return AOPFactory.CreateInstance<BranchCompanyDao>().ChangeStatus(id, status);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("更新分公司状态发生异常.", ex.Message));
                return -200;
            }
        }
        #endregion

        #region 修改分公司
        /// <summary>
        /// 修改分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-06
        /// </remarks>
        /// <param name="id">分公司id</param>
        /// <returns></returns>
        public BranchCompany GetBranchCompanyByID(int id)
        {
            try
            {
                return AOPFactory.CreateInstance<BranchCompanyDao>().GetBranchCompanyByID(id);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("根据分公司id获取分公司发生异常.{0}", ex.Message));
                return null;
            }
        }
        #endregion

        #region 修改分公司
        /// <summary>
        /// 修改分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-05-06
        /// </remarks>
        /// <param name="branchCompany">分公司实体</param>
        /// <returns></returns>
        public int Edit(BranchCompany branchCompany)
        {
            try
            {
                return AOPFactory.CreateInstance<BranchCompanyDao>().Update(branchCompany);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("修改分公司发生异常.", ex.Message));
                return -200;
            }
        }
        #endregion
    }
}

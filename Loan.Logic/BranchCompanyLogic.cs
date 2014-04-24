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
                _logger.Error(string.Format("新增分公司发生异常."));
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
                _logger.Error(string.Format("获取分公司列表发生异常."));
                return null;
            }
        }
        #endregion
    }
}

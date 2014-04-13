using Common.Logging;
using Loan.Core;
using Loan.Data;
using Loan.Entity.DB;
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

        /// <summary>
        /// 新增分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04013
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
    }
}

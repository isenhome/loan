using Loan.Entity.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Data
{
    public class BranchCompanyDao : BaseDao
    {
        #region 新增分公司
        /// <summary>
        /// 新增分公司
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04-13
        /// </remarks>
        /// <param name="branchCompany">分公司实体</param>
        /// <returns></returns>
        public int Insert(BranchCompany branchCompany)
        {
            var info = database.Insert("branchcompany", "branchcompanyID", true, branchCompany);
            if (info != null)
                return Convert.ToInt32(info);
            return -200;
        }
        #endregion
    }
}

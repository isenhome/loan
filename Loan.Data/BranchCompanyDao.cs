using Loan.Entity.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Core.Models;
using Loan.Core;
using PetaPoco;

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
            var result = database.Insert("branchcompany", "branchcompanyID", true, branchCompany);
            return result.TryInt();
        }
        #endregion

        #region 获取分公司列表
        /// <summary>
        /// 获取分公司列表
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-04-14
        /// </remarks>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public Page<BranchCompany> GetList(int pageSize, int pageIndex)
        {
            var sql = PetaPoco.Sql.Builder.Append("SELECT * FROM branchcompany ORDER BY BranchCompanyID ASC");
            return database.Page<BranchCompany>(pageIndex, pageSize, sql);
        }
        #endregion
    }
}

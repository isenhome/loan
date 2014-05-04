using Loan.Entity.DB;
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
            string strSql = string.Format("UPDATE branchcompany SET status=@status where branchcompanyid=@id");
            SqlParameter[] paras = new SqlParameter[] 
            { 
                new SqlParameter("@status", status.TryInt())
                , new SqlParameter("@id", id) 
            };

            return database.Update<BranchCompany>(strSql, paras);
        }
        #endregion
    }
}

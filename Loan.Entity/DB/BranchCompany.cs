using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Entity.DB
{
    [PetaPoco.TableName("branchcompany")]
    public class BranchCompany
    {
        public BranchCompany()
        {
        }

        /// <summary>
        /// 分公司ID
        /// </summary>
        [PetaPoco.Column]
        public int BranchCompanyID { get; set; }

        /// <summary>
        /// 分公司名称
        /// </summary>
        [PetaPoco.Column]
        public string Name { get; set; }

        /// <summary>
        /// 分公司地址
        /// </summary>
        [PetaPoco.Column]
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [PetaPoco.Column]
        public string PostCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [PetaPoco.Column]
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [PetaPoco.Column]
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loan.Web.Models
{
    public class BranchCompanyVModel
    {
        /// <summary>
        /// 分公司ID
        /// </summary>
        public int branchCompanyID { get; set; }
        
        /// <summary>
        /// 分公司名
        /// </summary>
        [Required(ErrorMessage = "请输入分公司名称")]
        [StringLength(25, MinimumLength=1, ErrorMessage="分公司名称不能超过25个字")]
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required(ErrorMessage = "请输入分公司地址")]
        [StringLength(100, MinimumLength=0, ErrorMessage="分公司地址不能超过100个字")]
        public string Address { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        [Required(ErrorMessage = "请输入邮编")]
        [RegularExpression("^[1-9][0-9]{5}$", ErrorMessage = "邮编格式不正确")]
        public string PostCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        #region 构造函数
        public BranchCompanyVModel() { }
        #endregion
    }
}
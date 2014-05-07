using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loan.Web.Models
{
    public class UsersVModel
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 分公司ID
        /// </summary>
        public int BranchCompanyID { get; set; }

        /// <summary>
        /// 用户类型ID
        /// </summary>
        public int UserTypeID { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入用户名")]
        [StringLength(32, MinimumLength=6, ErrorMessage="用户名长度必须在6-32个字符之间")]
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "请输入用户姓名")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "姓名长度必须在2-8个字之间")]
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required(ErrorMessage = "请选择性别")]
        public int Gender { get; set; }

        /// <summary>
        /// 员工号
        /// </summary>
        [Required(ErrorMessage = "请输入员工号")]
        [StringLength(32, MinimumLength = 2, ErrorMessage = "姓名长度必须在2-32之间")]
        public string EmployeeNO { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        [Required(ErrorMessage = "请输入员工手机号码")]
        [RegularExpression("^\\d{11}$", ErrorMessage = "手机格式不正确")]
        public string Cellphone { get; set; }

        #region 构造函数
        public UsersVModel() { }
        #endregion
    }
}
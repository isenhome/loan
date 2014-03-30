using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Loan.Web.Models
{
    public class LoginVModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入用户名")]
        [RegularExpression("^[a-zA-Z]\\w{5,15}$", ErrorMessage="用户名由6-16位字母、数字下划线组成，字母开头")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "请输入密码")]
        [DataType(DataType.Password)]
        [StringLength(16, MinimumLength=6, ErrorMessage="密码长度为6-16位")]
        public string Password { get; set; }

        ///// <summary>
        ///// 验证码
        ///// </summary>
        //[Required(ErrorMessage = "请输入验证码")]
        //[RegularExpression("^\\d{5}$", ErrorMessage = "验证码不正确")]
        //public string ValidCode { get; set; }

        #region 构造函数
        public LoginVModel() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-03-22
        /// </remarks>
        /// <param name="strUserName">用户名</param>
        /// <param name="strPassword">密码</param>
        public LoginVModel(string strUserName, string strPassword)
        {
            this.UserName = strUserName;
            this.Password = strPassword;
        }
        #endregion
    }
}

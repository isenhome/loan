using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Core
{
    public class GlobalConst
    {
        #region 全局常量
        /// <summary>
        /// 用户登陆时存储验证码的Session名称
        /// </summary>
        public const string SESSIONNAME_VALIDCODE_LOGIN = "ValidCode_Login";

        /// <summary>
        /// 用户注册时存储验证码的Session名称
        /// </summary>
        public const string SESSIONNAME_VALIDCODE_REGISTER = "ValidCode_Register";

        /// <summary>
        /// 记录登陆用户名的SESSION名称
        /// </summary>
        public const string SESSIONNAME_USERNAME = "UserName";
        #endregion

        #region 枚举
        public enum Status
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 0,

            /// <summary>
            /// 禁用、锁定
            /// </summary>
            Locked = 1,

            /// <summary>
            /// 删除
            /// </summary>
            Delete = 2
        }
        #endregion
    }
}

using Loan.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Entity.DB
{
    [PetaPoco.TableName("Users")]
    public class Users
    {
        public Users()
        {
            this.Status = ObjectExtension.TryInt(GlobalConst.Status.Normal);
            this.CreateTime = DateTime.Now;
            this.UpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [PetaPoco.Column]
        public int UserID { get; set; }

        /// <summary>
        /// 所属分公司ID
        /// </summary>
        [PetaPoco.Column]
        public int BranchCompanyID { get; set; }
        
        /// <summary>
        /// 用户类型ID
        /// </summary>
        [PetaPoco.Column]        
        public int UserTypeID { get; set; }
        
        /// <summary>
        /// 用户名
        /// </summary>
        [PetaPoco.Column]
        public string UserName { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        [PetaPoco.Column]
        public string Password { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [PetaPoco.Column]
        public string RealName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [PetaPoco.Column]
        public int Gender { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        [PetaPoco.Column]
        public string EmployeeNO { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [PetaPoco.Column]
        public string Cellphone { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        [PetaPoco.Column]
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 更新时间
        /// </summary>
        [PetaPoco.Column]
        public DateTime UpdateTime { get; set; }
        
        /// <summary>
        /// 状态
        /// </summary>
        [PetaPoco.Column]
        public int Status { get; set; }
    }
}

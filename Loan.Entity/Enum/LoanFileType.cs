using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Entity.Enum
{
    public enum LoanFileType
    {
        [Description("未知")]
        None = 0,
        [Description("身份证资料")]
        Identity = 1
    }
}

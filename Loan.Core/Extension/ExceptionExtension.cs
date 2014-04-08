using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loan.Core.Extension
{
    public static class ExceptionExtention
    {
        public static Exception ToCatchException(this Exception ex)
        {
            return new Exception("Catch后重新抛出异常:" + ex.Message, ex);
        }
    }
}

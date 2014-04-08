using System;

namespace Loan.Core.Helper
{
    public static class Md5Code
    {
        public static String AcMd5(String str, Int32 code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }

            if (code == 32) //32位加密 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }

            return "00000000000000000000000000000000";
        }
    }
}

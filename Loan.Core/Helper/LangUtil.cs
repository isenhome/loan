using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loan.Core.Helper
{
    public class LangUtil
    {
        public static string GetLangCode()
        {
            return LangHelper.GetLangCode();
        }
        public static string Lang(string key)
        {
            return LangHelper.Lang(key);
        }
        /// <summary>
        /// 直接翻译中文，需要在trans.xml中加入文本的多语言项
        /// </summary>
        /// <param name="text">文本</param>
        /// <returns></returns>
        public static string Trans(string text)
        {
            return LangHelper.Trans(text);
        }
    }
}

using System;
using System.Text;

namespace Loan.Core.Helper
{
    public class Util
    {
        public static String SqlFilter(String param)
        {
            return param
                .Replace("--", "")
                .Replace("'", "")
                .Replace("=", "")
                .Replace(" or ", "")
                .Replace(" and ", "");
        }

        private String[] majorNames = { "千", "万", "亿" };
        private String[] tenDoubleNams = { "", "一十", "二十", "三十", "四十", "五十", "六十", "七十", "八十", "九十" };
        private String[] tenNames = { "", "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八", "十九" };
        private String[] numberNames = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        /// <summary>
        /// 转超过1000的数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public String ConvertMoreThanThousand(Int32 number)
        {
            //越界
            if (number < 999)
            {
                throw new ArgumentException("number must be greater than 999");
            }

            String result = String.Empty;

            //处理千分位
            String strNumber = number.ToString();
            Int32 numLen = strNumber.Length;
            Int32 tempNumber = Convert.ToInt32(strNumber.Substring(numLen - 4, 1));   //取千位
            Int32 lessThousand = Convert.ToInt32(strNumber.Substring(numLen - 3));    //千位之后所有位
            result = numberNames[tempNumber] + "千";

            if (lessThousand == 0)
            {
                //后面全为0
            }
            else if (lessThousand < 999)   //二位数，要补零
            {
                result += "零" + ConvertLessThanThousand(lessThousand);
            }
            else
            {
                result += ConvertLessThanThousand(lessThousand);
            }

            //千分位之上的数字，每4位作为一段进行处理
            strNumber = strNumber.Remove(numLen - 4);
            numLen = strNumber.Length;  //重新计算长度
            Int32 index = 1;
            String tempCalculate = String.Empty;

            while (numLen > 0 && index < majorNames.Length)
            {

                tempCalculate = strNumber.Substring((numLen - 4) > 0 ? numLen - 4 : 0); //最后4位;

                if (Convert.ToInt32(tempCalculate) < 1000)
                {
                    tempCalculate = ConvertLessThanThousand(Convert.ToInt32(tempCalculate));
                }
                else
                {
                    tempCalculate = ConvertMoreThanThousand(Convert.ToInt32(tempCalculate));
                }

                result = tempCalculate + majorNames[index] + result;
                strNumber = strNumber.Remove((numLen - 4) > 0 ? numLen - 4 : 0);
                numLen = strNumber.Length;
                index++;
            }

            return result;
        }

        /// <summary>
        /// 转小于1000 (0-999) 的数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public String ConvertLessThanThousand(Int32 number)
        {
            //越界
            if (number < 0 || number > 999)
            {
                throw new ArgumentException("number must between 0 and 999");
            }

            String result = String.Empty;

            if (number < 100)
            {
                return ConvertLessThanHundred(number);
            }
            else     //大于100 (100-999)
            {
                Int32 tempNumber = number / 100;  //取百位
                result = numberNames[tempNumber] + "百";
                tempNumber = number - tempNumber * 100; //取个位与十位

                if (tempNumber == 0)    //tempNumber==0则就被整除
                {
                    //do nothing
                }
                else if (tempNumber < 10)    //存在零
                {
                    result += "零" + ConvertLessThanHundred(tempNumber);
                }
                else
                {
                    result += ConvertLessThanHundred(tempNumber);
                }
            }

            return result;
        }


        /// <summary>
        /// 转小于100 (0-99) 的数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public String ConvertLessThanHundred(Int32 number)
        {
            //越界
            if (number < 0 || number > 99)
            {
                throw new ArgumentException("number must between 0 and 99");
            }

            String result = String.Empty;   //存储返回结果
            //0-10之间
            if (number < 10)
            {
                result = numberNames[number];
            }
            else    //10-99
            {
                if (number % 10 == 0)   //10的倍数
                {
                    result = tenDoubleNams[number / 10];
                }
                else     //非10的倍数
                {
                    result = tenDoubleNams[number / 10] + numberNames[number % 10]; //分别从十的倍数和数字数组中取
                }
            }

            return result;
        }

        // 当SQL in(value1,value2,...valueN) 的N>1000时，返回一个新的SQL字句
        public static String GetSqlInClauseResolve1000Limit(String filedName, Int32[] values)
        {
            if (values == null || values.Length == 0) return "";
            if (values.Length <= 1000)
            {
                return String.Format(" {0} IN({1}) ", filedName, String.Join(",", values));
            }

            StringBuilder sb = new StringBuilder();
            Int32 count = values.Length / 1000;
            Int32 mod = values.Length % 1000;
            for (Int32 i = 0; i < count; i++)
            {
                Int32[] b = new Int32[1000];
                for (Int32 j = i * 1000; j < (i + 1) * 1000; j++)
                {
                    b[j % 1000] = values[j];
                }

                if (i == 0)
                {
                    sb.AppendFormat(" {0} IN({1}) ", filedName, String.Join(",", b));
                }
                else
                {
                    sb.AppendFormat(" OR {0} IN({1}) ", filedName, String.Join(",", b));
                }
            }

            if (mod > 0)
            {
                Int32[] c = new Int32[mod];
                for (Int32 i = 0; i < mod; i++)
                {
                    c[i] = values[i + count * 1000];
                }
                sb.AppendFormat(" OR {0} IN({1}) ", filedName, String.Join(",", c));
            }
            return sb.ToString();
        }
    }
}

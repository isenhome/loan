using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace Loan.Core.Extension
{
    /// <summary>
    /// String的扩展 oyster 2009-12-17
    /// </summary>
    public static class StringExtension
    {
        #region 验证

        /// <summary>
        /// 2012-05-29 add by terry
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] CommaStringToArray(this string str)
        {
            if (str == null) return null;

            string[] array = str.Split(',');
            return array;
        }


        /// <summary>
        /// 2012-05-29 add by terry
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static string ArrayToString(this string[] array)
        {
            string result = string.Empty;
            if (array == null) return result;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i] + ",";
            }
            result = result.TrimEnd(',');
            return result;
        }

        /// <summary>
        /// 是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(this string strText)
        {
            if (strText == null || strText == "") return false;
            return new Regex(@"^\d+|-\d+$").IsMatch(strText);
        }

        /// <summary>
        /// 是否是日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return false;
            }
            bool bk = false;
            Regex reg = new Regex("^\\d{4}-\\d{1,2}-\\d{1,2}(\\s*\\d{1,2}(:\\d{1,2}(:\\d{1,2})?)?)?$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (reg.IsMatch(date))
            {
                DateTime dt = DateTime.MinValue;
                if (DateTime.TryParse(date, out dt))
                {
                    bk = true;
                }
            }

            return bk;
        }
        /// <summary>
        /// 是否是16位整数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsInt16(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
            bool bk = false;
            Regex reg = new Regex("^-?\\d+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (reg.IsMatch(data))
            {
                Int16 dt = 0;
                if (Int16.TryParse(data, out dt))
                {
                    bk = true;
                }
            }

            return bk;
        }

        /// <summary>
        /// 是否是32位整数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsInt32(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
            bool bk = false;
            Regex reg = new Regex("^-?\\d+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (reg.IsMatch(data))
            {
                Int32 dt = 0;
                if (Int32.TryParse(data, out dt))
                {
                    bk = true;
                }
            }

            return bk;
        }

        /// <summary>
        /// 是否是64位整数
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsInt64(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
            bool bk = false;
            Regex reg = new Regex("^-?\\d+$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (reg.IsMatch(data))
            {
                Int64 dt = 0;
                if (Int64.TryParse(data, out dt))
                {
                    bk = true;
                }
            }

            return bk;
        }

        /// <summary>
        /// 是否是Decimal,一般用于验证带小数的数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
            bool bk = false;
            Regex reg = new Regex("^-?\\d+(\\.\\d+)?$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (reg.IsMatch(data))
            {
                Decimal dt = 0;
                if (Decimal.TryParse(data, out dt))
                {
                    bk = true;
                }
            }

            return bk;
        }

        /// <summary>
        /// 是否是Decimal,一般用于验证带小数的数字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsDouble(this string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }
            bool bk = false;
            Regex reg = new Regex("^-?\\d+(\\.\\d+)?$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (reg.IsMatch(data))
            {
                Double dt = 0;
                if (Double.TryParse(data, out dt))
                {
                    bk = true;
                }
            }

            return bk;
        }
        #endregion

        #region 扩展方法
        public static bool IsNullOrEmpty(this String str)
        {
            return String.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this String str)
        {
            return String.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 默认gb2312格式
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long BytesCount(this String str)
        {
            return str.BytesCount(Encoding.GetEncoding("gb2312"));
        }
        /// <summary>
        /// 字符串的gb2312的格式字节数 英文1 汉字2
        /// utf8 英文1 汉字3
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static long BytesCount(this String str, Encoding encode)
        {
            return encode.GetByteCount(str);
        }
        ///// <summary>
        ///// 根据字节进行裁剪 多用于数据库存储的字段超出 默认编码格式是gb2312
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="length"></param>
        ///// <returns></returns>
        //public static string SubStringByBytes(this String str, long length)
        //{
        //    return str.SubStringByBytes(length, Encoding.GetEncoding("gb2312"));
        //}
        /// <summary>
        /// 根据字节进行裁剪 多用于数据库存储的字段超出
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string SubStringByBytes(this String str, long length, Encoding encode)
        {
            string s = null;
            byte[] bs = encode.GetBytes(str);
            //在设定的长度以内直接返回
            if (bs.Length <= length)
            {
                return str;
            }
            char[] cs = encode.GetChars(bs, 0, (int)length);
            int lsb = (int)cs[cs.Length - 1];
            s = new string(cs);
            long l = s.BytesCount(encode);

            if (lsb <= 255 && bs[l - 1] > 127)
            {
                s = s.Substring(0, s.Length - 1);
            }

            return s;
        }

        /// <summary>
        /// 根据字节进行裁剪 多用于数据库存储的字段超出 默认编码格式是gb2312
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubStringByBytes(this String str, long length)
        {
            return str.SubStringByBytes(length, Encoding.GetEncoding("gb2312"), false);
        }
        /// <summary>
        /// 根据字节进行裁剪 多用于数据库存储的字段超出 默认编码格式是gb2312
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubStringByBytes(this String str, long length, bool useEllipsis)
        {
            return str.SubStringByBytes(length, Encoding.GetEncoding("gb2312"), useEllipsis);
        }
        /// <summary>
        /// 根据字节进行裁剪 多用于数据库存储的字段超出
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string SubStringByBytes(this String str, long length, Encoding encode, bool useEllipsis)
        {
            if (str == null)
            {
                return "";
            }

            string s = null;
            byte[] bs = encode.GetBytes(str);
            //在设定的长度以内直接返回
            if (bs.Length <= length)
            {
                return str;
            }
            char[] cs = encode.GetChars(bs, 0, (int)length);
            int lsb = (int)cs[cs.Length - 1];
            s = new string(cs);
            long l = s.BytesCount(encode);

            if (lsb <= 255 && bs[l - 1] > 127)
            {
                s = s.Substring(0, s.Length - 1);
            }
            return useEllipsis ? (s + "...") : s;
        }

        /// <summary>
        /// 根据分隔符返回相应数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static string[] ToSplitArray(this string value, char splitStr)
        {
            if (string.IsNullOrEmpty(value)) return new string[] { };
            string[] items = null;
            items = value.Split(splitStr);
            return items;
        }

        /// <summary>
        /// 将字符串数组转换成整形数组，如果转换失败的为0
        /// </summary>
        /// <param name="sArray"></param>
        /// <returns></returns>
        public static int[] ToIntergerArray(this string[] sArray)
        {
            int[] iArray = new int[sArray.Length];
            for (int i = 0; i < sArray.Length; i++)
            {
                iArray[i] = sArray[i].TryInt();
            }
            return iArray;
        }

        /// <summary>
        /// 根据分隔符返回相应数组
        /// </summary>
        /// <param name="value"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static int[] ToSplitIntArray(this string value, char splitStr)
        {
            string[] items = ToSplitArray(value, splitStr);
            return items.ToIntergerArray();
        }

        /// <summary>
        /// MD5加密字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this String input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
            char[] temp = new char[res.Length];
            System.Array.Copy(res, temp, res.Length);
            return new String(temp);
        }

        #endregion


        public static string VaryLogicString(this string str)
        {
            string result = "";
            str = str.Replace("!", "!-");
            var array = str.Split(new string[] { "&&", "!" }, StringSplitOptions.RemoveEmptyEntries);
            var ands = new List<string>();
            var resultList = new List<string>();
            for (var item = 0; item < array.Length; item++)
            {
                var tempList = new List<string>();
                if (array[item].IndexOf("-") == 0)
                {
                    var interver = array[item].Replace("-", "").Replace("||", "&&").Replace("{", "!{").Replace("(", "").Replace(")", "");
                    if (resultList.Count == 0)
                    {
                        tempList.Add(interver);
                    }
                    else
                    {
                        for (var i = 0; i < resultList.Count; i++)
                        {
                            tempList.Add(resultList[i] + "&&" + interver);
                        }
                    }
                }
                else
                {
                    Regex regex = new Regex(@"\{(\d{1,})\}");
                    var matchs = regex.Matches(array[item]);
                    if (resultList.Count == 0)
                    {
                        foreach (var match in matchs)
                        {
                            tempList.Add(match.ToString());
                        }
                    }
                    else
                    {
                        for (var i = 0; i < resultList.Count; i++)
                        {
                            
                            foreach (var match in matchs)
                            {
                                tempList.Add(resultList[i] + "&&" + match.ToString());
                            }
                        }
                    }
                }
                resultList = tempList;
            }
            result = String.Join(")||(", resultList);
            result = "(" + result + ")";
            return result;
        }

    }
}

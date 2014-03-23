using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Loan.Core
{
    public static class ObjectExtension
    {
        #region 转化

        /// <summary>
        /// 转换为Int，默认值：0
        /// </summary>
        public static Int32 TryInt(this Object strText)
        {
            return TryInt(strText, 0);
        }

        public static Int64 TryLong(this Object strText)
        {
            return TrtLong(strText, 0);
        }

        public static Int64 TrtLong(this object strText, Int64 defValue)
        {
            Int64 temp = -9999;
            return long.TryParse(strText + "", out temp) ? temp : defValue;
        }

        /// <summary>
        /// 转换为Int
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Int32 TryInt(this Object strText, Int32 defValue)
        {
            int temp = -9999;
            if (strText == null) strText = "";
            return int.TryParse(strText + "", out temp) ? temp : defValue;
        }

        /// <summary>
        /// 转换为Double，默认值：0
        /// </summary>
        public static Double TryDouble(this Object strText)
        {
            return TryDouble(strText, 0.0);
        }

        /// <summary>
        /// 转换为Double
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Double TryDouble(this Object strText, Double defValue)
        {
            double temp = -9999.0;
            return double.TryParse(strText + "", out temp) ? temp : defValue;
        }

        /// <summary>
        /// 转换为Boolean，默认值：false
        /// </summary>
        public static Boolean TryBoolean(this Object strText)
        {
            return TryBoolean(strText, false);
        }

        /// <summary>
        /// 转换为Boolean
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Boolean TryBoolean(this Object strText, Boolean defValue)
        {
            bool temp = false;
            return bool.TryParse(strText + "", out temp) ? temp : defValue;
        }

        /// <summary>
        /// 转换为Decimal，默认值：0
        /// </summary>
        public static Decimal TryDecimal(this Object strText)
        {
            return TryDecimal(strText, 0);
        }

        /// <summary>
        /// 转换为Decimal
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static Decimal TryDecimal(this Object strText, Decimal defValue)
        {
            decimal temp = -9999;
            return decimal.TryParse(strText + "", out temp) ? temp : defValue;
        }

        /// <summary>
        /// 转换为DateTime，默认值：1900-1-1
        /// </summary>
        public static DateTime TryDateTime(this Object strText)
        {
            return TryDateTime(strText, DateTime.Parse("1900-1-1"));
        }

        /// <summary>
        /// 转换为Int
        /// </summary>
        /// <param name="strText"></param>
        /// <param name="defValue">默认值</param>
        /// <returns></returns>
        public static DateTime TryDateTime(this Object strText, DateTime defValue)
        {
            DateTime temp = DateTime.Parse("1900-1-1");
            return DateTime.TryParse(strText + "", out temp) ? temp : defValue;
        }

        /// <summary>
        /// 为NULL 和 DBNull的返回String.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String TryString(this Object str)
        {
            return TryString(str, "");
        }

        /// <summary>
        /// 转换为""
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String TryString(this Object str, String defaultValue)
        {
            return str == null ? defaultValue : str.ToString();
        }

        public static Int32[] TryIntArray(this String str)
        {
            Int32[] result = new Int32[] { };
            result = str.Split(new char[] { ',' }).Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => Convert.ToInt32(p)).ToArray();
            return result;
        }

        public static Int64[] TryLongArray(this String str)
        {
            Int64[] result = new Int64[] { };
            result = str.Split(new char[] { ',' }).Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => Convert.ToInt64(p)).ToArray();
            return result;
        }

        public static HashSet<Int64> TryLongHashSet(this String str)
        {
            HashSet<Int64> result = new HashSet<Int64>();
            if (string.IsNullOrWhiteSpace(str)) return result;

            foreach (var item in str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                result.Add(item.TryLong());
            }
            return result;
        }

        public static String ParString(this String str)
        {
            Type t = null;
            return ParseString(str, out t).ToString();
        }

        public static Object ParseString(this String str, out Type type)
        {
            Object o = null;
            Type t = null;

            if (str.IndexOf(".") > -1 && str.IndexOf("%") <= -1 && System.Text.RegularExpressions.Regex.IsMatch(str, "[A-Z]") == false && System.Text.RegularExpressions.Regex.IsMatch(str, "[a-z]") == false)
            {
                o = float.Parse(str.Replace(",", String.Empty));
                t = typeof(float);
            }
            else
            {
                DateTime dt;
                Boolean flag = DateTime.TryParse(str.Replace(",", String.Empty), out dt);

                if (flag)
                {
                    o = dt.ToString("yyyy/MM/dd");
                    t = typeof(String);
                }
                else
                {
                    if (str.IndexOf("%") > -1)
                    {
                        o = Double.Parse(str.Replace("%", String.Empty).Replace(",", String.Empty)) / (Double)100;
                        t = typeof(Double);
                    }
                    else if (str.IndexOf(",") > -1)
                    {
                        Int32 value = 0;
                        Boolean flag2 = Int32.TryParse(str.Replace(",", String.Empty), out value);

                        if (flag2)
                        {
                            o = value;
                            t = typeof(Int32);
                        }
                        else
                        {
                            o = str;
                            t = typeof(String);
                        }
                    }
                    else
                    {
                        Int32 temp = 0;
                        Boolean flag3 = Int32.TryParse(str.Replace(",", String.Empty), out temp);

                        if (flag3)
                        {
                            o = temp;
                            t = typeof(Int32);
                        }
                        else
                        {
                            o = str;
                            t = typeof(String);
                        }
                    }
                }
            }

            type = t;
            return o;
        }

        public static String ToThousands(this Int32 value)
        {
            return value.ToString("N0");
        }

        public static String ToThousands(this Int64 value)
        {
            return value.ToString("N0");
        }

        public static String RemoveThousands(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            if (value.IndexOf("%") > -1)
            {
                value = value.Replace("%", "");
            }

            Double a = 0;
            Boolean temp = Double.TryParse(value.Replace(",", String.Empty), out a);

            if (temp)
            {
                return a.ToString();
            }

            return value;
        }

        public static String ToThousands(this Double value)
        {
            return value.ToString("N0");
        }

        public static String ToThousands(this Decimal value)
        {
            return value.ToString("N0");
        }

        public static String ToThousands(this Decimal? value)
        {
            return value == null ? "-" : value.Value.ToString("N0");
        }

        public static String ToDataTime(this Double value)
        {
            return String.Format("{0,21}", new TimeSpan(0, 0, (Int32)value)).Trim();
        }

        public static String RemoveDash(this String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return "0";
            }

            if (value.Trim().Length == 1)
            {
                return value.Replace("-", "0");
            }

            return value;
        }

        public static HashSet<String> MergeWith(this HashSet<String> hs1, HashSet<String> hs2)
        {
            if (hs1 == null)
            {
                return new HashSet<String>();
            }

            if (hs2 == null)
            {
                return hs1;
            }

            foreach (var item in hs2)
            {
                hs1.Add(item);
            }

            return hs1;
        }

        /// <summary>
        /// 将FormatEmptyStr,GetSubstring和Tooltip,ReplaceKeyword合并后的方法
        /// </summary>
        /// <param name="content"></param>
        /// <param name="length"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string ToTooltipString(this string content, int length = 0, string keyword = "", string toolTip = "")
        {
            if (string.IsNullOrEmpty(content))
            {
                return content.FormatEmptyStr();
            }
            else
            {
                return string.Format("<span title=\"{0}\">{1}</span>", toolTip != "" ? toolTip : TransHtmlSymbol(content), content.GetSubString(length).ReplaceKeyword(keyword));
            }
        }

        ////sharping add by 2010-8-10
        /// <summary>
        /// 格式化空串,返回指定字符串，默认是"--"
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static string FormatEmptyStr(this string inputStr)
        {
            return FormatEmptyStr(inputStr, "--");
        }

        /// <summary>
        /// 格式化空串,返回指定字符串outputStr
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="outputStr"></param>
        /// <returns></returns>
        public static string FormatEmptyStr(this string inputStr, string outputStr)
        {
            if (inputStr == null || inputStr.Trim() == string.Empty)
            {
                inputStr = outputStr;
            }
            return inputStr;
        }

        /// <summary>
        /// 给需要转义属性内容的属性进行转义
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string TransHtmlSymbol(string content)
        {
            return content.Replace("\"", @"&quot;").Replace("<", @"&lt;").Replace(">", @"&gt;");
        }

        /// <summary>
        /// 获取指定字节长度的中英文混合字符串
        /// </summary>
        public static string GetSubString(this string str, int len)
        {
            if (len <= 0) return str;
            string result = string.Empty;// 最终返回的结果
            if (!string.IsNullOrEmpty(str))
            {
                int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
                int charLen = str.Length;// 把字符平等对待时的字符串长度
                int byteCount = 0;// 记录读取进度
                int pos = 0;// 记录截取位置
                if (byteLen > len)
                {
                    for (int i = 0; i < charLen; i++)
                    {
                        if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                            byteCount += 2;
                        else// 按英文字符计算加1
                            byteCount += 1;
                        if (byteCount > len)// 超出时只记下上一个有效位置
                        {
                            pos = i;
                            break;
                        }
                        else if (byteCount == len)// 记下当前位置
                        {
                            pos = i + 1;
                            break;
                        }
                    }

                    if (pos >= 0)
                        result = str.Substring(0, pos) + "..";
                }
                else
                    result = str;
            }
            return result;
        }

        /// <summary>
        /// 替换关键字
        /// </summary>
        /// <param name="content"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static string ReplaceKeyword(this string content, string keyword)
        {
            //content = content.ToHTML();

            if (string.IsNullOrEmpty(keyword))
            {
                return content;
            }
            else
            {
                MatchEvaluator evaluator = new MatchEvaluator(GetReplacement);
                return Regex.Replace(content, keyword, evaluator, RegexOptions.IgnoreCase);
            }
        }

        private static string GetReplacement(Match m)
        {
            return string.Format("<span style='color:#ff9900'>{0}</span>", m.Value);
        }

        /// <summary>
        /// 去掉文件名中的无效字符,如 \ / : * ? " < > | 
        /// </summary>
        /// <param name="fileName">待处理的文件名</param>
        /// <returns>处理后的文件名</returns>
        public static string FormatFileName(this string fileName)
        {
            string str = fileName;
            str = str.Replace("\\", string.Empty);
            str = str.Replace("/", string.Empty);
            str = str.Replace(":", string.Empty);
            str = str.Replace("*", string.Empty);
            str = str.Replace("?", string.Empty);
            str = str.Replace("\"", string.Empty);
            str = str.Replace("<", string.Empty);
            str = str.Replace(">", string.Empty);
            str = str.Replace("|", string.Empty);
            str = str.Replace(" ", string.Empty);    //前面的替换会产生空格,最后将其一并替换掉
            return str;
        }

        #endregion

        #region time
        private static long lLeft = 621355968000000000;
        //将数字变成时间
        public static DateTime GetTimeFromInt(this long ltime)
        {
            long Eticks = (long)(ltime * 10000000) + lLeft;
            DateTime dt = new DateTime(Eticks).ToLocalTime();
            return dt;

        }
        //将时间变成数字
        public static long GetIntFromTime(this DateTime dt)
        {
            dt = dt.ToShortDateString().TryDateTime();
            DateTime dt1 = dt.AddDays(1).ToUniversalTime();
            long Sticks = (dt1.Ticks - lLeft) / 10000;
            return Sticks;
        }

        public static string FormatVisitTime(this double seconds)
        {
            var time = "00:00:00".TryDateTime().AddSeconds(seconds);
            var str = string.Format("{0}:{1}:{2}", time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'), time.Second.ToString().PadLeft(2, '0'));
            return str;
        }

        public static string FormatVisitTime(this int seconds)
        {
            var time = "00:00:00".TryDateTime().AddSeconds(seconds);
            var str = string.Format("{0}:{1}:{2}", time.Hour.ToString().PadLeft(2, '0'), time.Minute.ToString().PadLeft(2, '0'), time.Second.ToString().PadLeft(2, '0'));
            return str;
        }

        #endregion

        #region 验证码相关
        //// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public static string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="containsPage">要输出到的page对象</param>
        /// <param name="validateNum">验证码</param>
        public static byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 14.0), 38);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 14, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 7);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion

        #region 获取唯一标识(GUID)
        /// <summary>
        /// 获取唯一标识(GUID)
        /// </summary>
        /// <remarks>
        /// 创建：李真 2014-03-22
        /// </remarks>
        /// <returns>GUID</returns>
        public static string GetNewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        #endregion
    }
}

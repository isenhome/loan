using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Loan.Core.Helper
{
    /// <summary>
    /// Des加密解密类
    /// </summary>
    public class DesEncryptHelper
    {
        private string _sKey;//加密密钥
        private string _sIV;//IV向量

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sIV"></param>
        public DesEncryptHelper(string sKey)
        {
            _sKey = sKey;
            _sIV = sKey;
            //todo:job 
        }
        #endregion

        #region 加密方法
        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="strToEncrypt">待Des加密的字符串</param>
        /// <returns>返回已经加密的字符串</returns>
        public string strDesEncrypt(string strToEncrypt)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strToEncrypt);
                des.Mode = CipherMode.ECB;
                des.Key = getKey(Encoding.UTF8.GetBytes(_sKey));
                des.IV = getKey(Encoding.UTF8.GetBytes(_sIV));
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
                byte[] bytResult = ms.ToArray();
                ms.Close();
                des.Clear();
                return byteArr2HexStr(bytResult);
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region Des解密方法
        /// <summary>
        /// Des解密方法
        /// </summary>
        /// <param name="pToDecrypt">待解密的字符串</param>
        /// <returns>返回解密后的字符串</returns>
        public string strDesDecrypt(string strToDecrypt)
        {
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[strToDecrypt.Length / 2];
                for (int x = 0; x < strToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(strToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Mode = CipherMode.ECB;
                des.Key = getKey(ASCIIEncoding.UTF8.GetBytes(_sKey));
                des.IV = getKey(ASCIIEncoding.UTF8.GetBytes(_sIV));
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
                ms.Close();
                des.Clear();
                return System.Text.Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region byte数组与字符串进行互相转换
        /// <summary>
        /// 将byte数组转换为表示16进制值的字符串
        /// </summary>
        /// <param name="arrB">需要转换的byte数组</param>
        /// <returns>转换后的字符串</returns>
        private string byteArr2HexStr(byte[] arrB)
        {
            int iLen = arrB.Length;
            //每个byte用两个字符才能表示，所以字符串的长度是数组长度的两倍
            StringBuilder sb = new StringBuilder(iLen * 2);
            for (int i = 0; i < iLen; i++)
            {
                int intTmp = arrB[i];
                //把负数转换为正数
                while (intTmp < 0)
                {
                    intTmp = intTmp + 256;
                }
                //小于0F的数需要在前面补0
                if (intTmp < 16)
                {
                    sb.Append("0");
                }
                sb.Append(intTmp.ToString("x"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将十六进制的字符串转换成byte数组，每两位字符转换成一个byte值
        /// </summary>
        /// <param name="strIn">待转换的十六进制字符串</param>
        /// <returns>转换后的byte数组</returns>
        private byte[] hexStr2ByteArr(string strIn)
        {
            int iLen = strIn.Length;
            //两个字符表示一个字节，所以字节数组长度是字符串长度除以2
            byte[] arrOut = new byte[iLen / 2];
            for (int i = 0; i < iLen; i = i + 2)
            {
                string strTmp = strIn.Substring(i, 2);
                arrOut[i / 2] = (byte)Convert.ToInt32(strTmp, 16);
            }
            return arrOut;
        }
        #endregion

        #region 对Des的密钥进行限制
        /// <summary>
        /// 对Des的密钥进行限制（限制密钥的byte数组的长度为８)
        /// </summary>
        /// <param name="arrBTmp">密钥的byte数组</param>
        /// <returns>返回密钥的byte数组</returns>
        private byte[] getKey(byte[] arrBTmp)
        {
            //创建一个空的8位字节数组（默认值为0）
            byte[] arrB = new byte[8];
            //将原始字节数组转换为8位
            for (int i = 0; i < arrBTmp.Length && i < arrB.Length; i++)
            {
                arrB[i] = arrBTmp[i];
            }
            return arrB;
        }
        #endregion
    }
}

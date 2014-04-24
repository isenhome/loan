using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Loan.Core.Helper
{
    /// <summary>
    /// Des���ܽ�����
    /// </summary>
    public class DesEncryptHelper
    {
        private string _sKey;//������Կ
        private string _sIV;//IV����

        #region ���캯��
        /// <summary>
        /// ���캯��
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

        #region ���ܷ���
        /// <summary>
        /// ���ܷ���
        /// </summary>
        /// <param name="strToEncrypt">��Des���ܵ��ַ���</param>
        /// <returns>�����Ѿ����ܵ��ַ���</returns>
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

        #region Des���ܷ���
        /// <summary>
        /// Des���ܷ���
        /// </summary>
        /// <param name="pToDecrypt">�����ܵ��ַ���</param>
        /// <returns>���ؽ��ܺ���ַ���</returns>
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

        #region byte�������ַ������л���ת��
        /// <summary>
        /// ��byte����ת��Ϊ��ʾ16����ֵ���ַ���
        /// </summary>
        /// <param name="arrB">��Ҫת����byte����</param>
        /// <returns>ת������ַ���</returns>
        private string byteArr2HexStr(byte[] arrB)
        {
            int iLen = arrB.Length;
            //ÿ��byte�������ַ����ܱ�ʾ�������ַ����ĳ��������鳤�ȵ�����
            StringBuilder sb = new StringBuilder(iLen * 2);
            for (int i = 0; i < iLen; i++)
            {
                int intTmp = arrB[i];
                //�Ѹ���ת��Ϊ����
                while (intTmp < 0)
                {
                    intTmp = intTmp + 256;
                }
                //С��0F������Ҫ��ǰ�油0
                if (intTmp < 16)
                {
                    sb.Append("0");
                }
                sb.Append(intTmp.ToString("x"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// ��ʮ�����Ƶ��ַ���ת����byte���飬ÿ��λ�ַ�ת����һ��byteֵ
        /// </summary>
        /// <param name="strIn">��ת����ʮ�������ַ���</param>
        /// <returns>ת�����byte����</returns>
        private byte[] hexStr2ByteArr(string strIn)
        {
            int iLen = strIn.Length;
            //�����ַ���ʾһ���ֽڣ������ֽ����鳤�����ַ������ȳ���2
            byte[] arrOut = new byte[iLen / 2];
            for (int i = 0; i < iLen; i = i + 2)
            {
                string strTmp = strIn.Substring(i, 2);
                arrOut[i / 2] = (byte)Convert.ToInt32(strTmp, 16);
            }
            return arrOut;
        }
        #endregion

        #region ��Des����Կ��������
        /// <summary>
        /// ��Des����Կ�������ƣ�������Կ��byte����ĳ���Ϊ��)
        /// </summary>
        /// <param name="arrBTmp">��Կ��byte����</param>
        /// <returns>������Կ��byte����</returns>
        private byte[] getKey(byte[] arrBTmp)
        {
            //����һ���յ�8λ�ֽ����飨Ĭ��ֵΪ0��
            byte[] arrB = new byte[8];
            //��ԭʼ�ֽ�����ת��Ϊ8λ
            for (int i = 0; i < arrBTmp.Length && i < arrB.Length; i++)
            {
                arrB[i] = arrBTmp[i];
            }
            return arrB;
        }
        #endregion
    }
}

using System;
using System.DirectoryServices.Protocols;
using System.IO;
using System.Net;

namespace Loan.Core.Helper
{
    public static class FTPHelper
    {
        /// <summary>
        /// static  method  to upload  file to ftp
        /// </summary>
        /// <param name="FTPAddress">ftp  address:string</param>
        /// <param name="fileStream"></param>
        /// <param name="username">user name:string</param>
        /// <param name="password">password:string</param>
        /// <param name="directoryName">the storage file floder name:string</param>
        /// <param name="fileStorageName">the storage file's name:string</param>
        /// <history>Create  by  ben</history>
        /// <returns>
        /// if suceess  return  empty,else  return  error  msg
        /// </returns>
        public static string UploadCreativeToFTP(string FTPAddress, Stream fileStream, string username, string password, string directoryName, string fileStorageName)
        {
            if (CreateFtpListDirectory(FTPAddress, directoryName, username, password))
            {
                try
                {
                    directoryName = directoryName.TrimEnd('/');//修改20100420,"//"出错 
                    Uri BaseUri = new Uri(FTPAddress);
                    Uri AimUri = new Uri(BaseUri, directoryName + "/" + fileStorageName);

                    FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(AimUri);


                    FtpRequest.KeepAlive = true;
                    FtpRequest.ReadWriteTimeout = 60000;
                    FtpRequest.UsePassive = true;



                    NetworkCredential FtpCred = new NetworkCredential(username, password);
                    CredentialCache FtpCache = new CredentialCache();
                    FtpCache.Add(AimUri, AuthType.Basic.ToString(), FtpCred);
                    FtpRequest.Credentials = FtpCache;

                    FtpRequest.Method = WebRequestMethods.Ftp.UploadFile;

                    //Load the file
                    //FileStream stream = File.OpenRead(filePath);
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    Stream stm = FtpRequest.GetRequestStream();
                    stm.Write(buffer, 0, buffer.Length);
                    stm.Close();

                    FtpWebResponse FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
                    if (FtpResponse.StatusCode == FtpStatusCode.ClosingData)
                    {
                        FtpResponse.Close();
                        //FileInfo FI = new FileInfo(filePath);

                        if (GetFtpFileSize(FTPAddress, directoryName + "/" + fileStorageName, username, password) != fileStream.Length)
                            return "文件错误！";
                        //logger.Error("end upload 1");
                    }
                    else
                    {
                        FtpResponse.Close();
                        //logger.Error("end upload 2");
                    }
                }
                catch (WebException e)
                {
                    //logger.Error(e);

                    FtpWebResponse FtpResponse = (FtpWebResponse)e.Response;
                    FtpResponse.Close();
                    return "文件上传失败！";
                }
                finally
                {
                    fileStream.Position = 0;   //fileStream可能还会被别的方法再利用
                }
                //catch (Exception ex)
                //{
                //    logger.Error(ex);
                //}
            }
            else
                return "创建文件目录失败！";
            return string.Empty;

        }

        /// <summary>
        /// 创建文件夹:不实现级联创建
        /// 返回：true成功，false失败
        /// </summary>
        /// <param name="BaseUriStr">基Uri字符串：如ftp://192.168.1.190:21</param>
        /// <param name="AimUriStr">目标目录名，相对路径：如/FTF</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns></returns>
        public static bool CreateFtpDirectory(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            try
            {
                Uri BaseUri = new Uri(BaseUriStr);
                Uri AimUri = new Uri(BaseUri, AimUriStr);
                FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(AimUri);
                FtpRequest.KeepAlive = true;
                FtpRequest.Timeout = 60000;
                FtpRequest.UsePassive = false;
                NetworkCredential FtpCred = new NetworkCredential(UserName, UserPwd);
                CredentialCache FtpCache = new CredentialCache();
                FtpCache.Add(AimUri, AuthType.Basic.ToString(), FtpCred);
                FtpRequest.Credentials = FtpCache;
                FtpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;

                FtpWebResponse FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
                if (FtpResponse.StatusCode == FtpStatusCode.PathnameCreated)
                {
                    FtpResponse.Close();
                    //logger.Error("create ftp directory done true");
                    return true;
                }
                FtpResponse.Close();
                return false;
            }
            catch (WebException e)
            {
                //logger.Error(e);

                FtpWebResponse FtpResponse = (FtpWebResponse)e.Response;
                int result = (int)FtpResponse.StatusCode;
                if (result == (int)FtpStatusCode.ActionNotTakenFileUnavailable || result == 521)
                {
                    //文件已存在，返回True
                    FtpResponse.Close();
                    return true;
                }
                FtpResponse.Close();
                return false;
            }
            //catch (Exception ex)
            //{
            //    logger.Error(ex);
            //    return false;
            //}
        }

        /// <summary>
        /// 创建文件夹:实现级联创建
        /// 返回：true成功，false失败
        /// </summary>
        /// <param name="BaseUriStr">基Uri字符串：如ftp://192.168.1.190:21</param>
        /// <param name="AimUriStr">目标目录名，相对路径：如/FTF/FTF1/FTF2/FTF3</param>
        /// <param name="UserName">用户名</param>
        /// <param name="UserPwd">用户密码</param>
        /// <returns></returns>
        public static bool CreateFtpListDirectory(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            //logger.Error("begin creative ftp list dir");
            var pathString = AimUriStr.TrimStart('/');//修改20100420,AimUriStr="/"时出错
            if (!string.IsNullOrEmpty(pathString))
            {
                string[] AimUriArray = pathString.Split('/');
                string AimUriCache = string.Empty;
                for (int i = 0; i < AimUriArray.Length; i++)
                {
                    AimUriCache += "/" + AimUriArray[i];
                    if (CreateFtpDirectory(BaseUriStr, AimUriCache, UserName, UserPwd))
                    {
                        continue;
                    }
                    if (CreateFtpDirectory(BaseUriStr, AimUriCache, UserName, UserPwd))
                    {
                        continue;
                    }
                    //logger.Error("end create ftp list dir false");
                    return false;
                }
            }

            //logger.Error("end create ftp list dir true");
            return true;
        }

        public static int GetFtpFileSize(string BaseUriStr, string AimUriStr, string UserName, string UserPwd)
        {
            Uri BaseUri = new Uri(BaseUriStr);
            Uri AimUri = new Uri(BaseUri, AimUriStr);
            FtpWebRequest FtpRequest = (FtpWebRequest)WebRequest.Create(AimUri);
            FtpRequest.KeepAlive = true;
            FtpRequest.ReadWriteTimeout = 60000;
            FtpRequest.UsePassive = false;
            NetworkCredential FtpCred = new NetworkCredential(UserName, UserPwd);
            CredentialCache FtpCache = new CredentialCache();
            FtpCache.Add(AimUri, AuthType.Basic.ToString(), FtpCred);
            FtpRequest.Credentials = FtpCache;
            FtpRequest.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse FtpResponse = (FtpWebResponse)FtpRequest.GetResponse();
                long FileSize = FtpResponse.ContentLength;
                FtpResponse.Close();

                return Convert.ToInt32(FileSize);
            }
            catch (Exception ex)
            {
                //logger.Error(e);

                return -1;
            }
        }
    }
}


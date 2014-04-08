using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using Common.Logging;
using Loan.Core.Extension;

namespace Loan.Core.Helper
{
    public static class MailHelper
    {
        private static readonly String mailHost = "mailer.adchina.com";
        private static readonly String proxyMailAddress = "report@mailer.adchina.com";
        private static readonly NetworkCredential mailSecurity = new NetworkCredential("report", "8IGM5LN!n%H;1Z");
        private static ILog log = LogManager.GetLogger(typeof(MailHelper));

        public static void SendMail(String mailList, String subject, String body, String attachmentName)
        {
            SendMail(mailList, subject, body, new Byte[0], attachmentName);
        }

        public static void SendMail(String mailList, String subject, String body, Byte[] attachmentData, String attachmentName)
        {
            try
            {
                subject = ProcessMailSubject(subject);
                var mailClient = new SmtpClient(mailHost);
                mailClient.Credentials = mailSecurity;

                using (var mail = new MailMessage(proxyMailAddress, mailList, subject, body))
                {
                    mail.IsBodyHtml = true;

                    if (attachmentData != null && attachmentData.Length > 0)
                    {
                        mail.Attachments.Add(new Attachment(new MemoryStream(RARAttachmentData(attachmentData, attachmentName)), attachmentName + ".rar"));
                    }

                    mailClient.Send(mail);
                    log.Debug(String.Format("CampaignReport sented to:{0}", mailList));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex.ToCatchException();
            }
        }

        public static void SendMail(String mailList, String subject, String body, Dictionary<String, Byte[]> attchments)
        {
            try
            {
                subject = ProcessMailSubject(subject);
                var mailClient = new SmtpClient(mailHost);
                mailClient.Credentials = mailSecurity;

                using (var mail = new MailMessage(proxyMailAddress, mailList, subject, body))
                {
                    mail.IsBodyHtml = true;
                    mail.Attachments.Add(new Attachment(new MemoryStream(RARAttachmentData(attchments, subject)), subject + ".rar"));
                    mailClient.Send(mail);
                    log.Debug(String.Format("CampaignReport sented to:{0}", mailList));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex.ToCatchException();
            }
        }
        public static void SendMailNotExcel(String mailList, String subject, String body, Dictionary<String, Byte[]> attchments)
        {
            try
            {
                subject = ProcessMailSubject(subject);
                var mailClient = new SmtpClient(mailHost);
                mailClient.Credentials = mailSecurity;

                using (var mail = new MailMessage(proxyMailAddress, mailList, subject, body))
                {
                    mail.IsBodyHtml = true;
                    mail.Attachments.Add(new Attachment(new MemoryStream(RARAttachmentDataNotExcel(attchments, subject)), subject + ".rar"));
                    mailClient.Send(mail);
                    log.Debug(String.Format("CampaignReport sented to:{0}", mailList));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex.ToCatchException();
            }
        }

        /// <summary>
        /// 发送不压缩的文件
        /// </summary>
        /// <param name="mailList"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attchments"></param>
        public static void SendMailWithNoRar(String mailList, String subject, String body, string fileName)
        {
            try
            {
                subject = ProcessMailSubject(subject);
                var mailClient = new SmtpClient(mailHost);
                mailClient.Credentials = mailSecurity;

                using (var mail = new MailMessage(proxyMailAddress, mailList, subject, body))
                {
                    mail.IsBodyHtml = true;
                    mail.Attachments.Add(new Attachment(fileName));
                    mailClient.Send(mail);
                    log.Debug(String.Format("归因报告数据 sented to:{0}", mailList));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw ex.ToCatchException();
            }
        }

        public static String ProcessMailSubject(String subject)
        {
            return subject.Replace("\\", "_").Replace("/", "_").Replace(" ", "_");
        }

        private static Byte[] RARAttachmentData(Byte[] attachmentData, String attachmentFileName)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var toRarDirecotry = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var rarFile = Path.Combine(tempDirectory, "Report" + ".rar");
            var flyFile = Path.Combine(toRarDirecotry, attachmentFileName + ".xlsx");

            Directory.CreateDirectory(tempDirectory);
            Directory.CreateDirectory(toRarDirecotry);
            File.WriteAllBytes(flyFile, attachmentData);

            String rarArguments = @"a -m5 -ep ""{0}"" ""{1}""";
            rarArguments = String.Format(
                rarArguments,
                rarFile,
                toRarDirecotry + "\\*"
            );

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = @"Rar.exe",
                Arguments = rarArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            });

            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception("Run WinRAR RAR Command Failed!, Exit Code: " + proc.ExitCode +
                    ", Details StandardOuput: " + proc.StandardOutput.ReadToEnd() +
                    ",StandardError" + proc.StandardError.ReadToEnd() +
                    ",StandardInput" + proc.StandardInput +
                    ",Site" + proc.Site);
            }
            else
            {
                var contents = File.ReadAllBytes(rarFile);
                Directory.Delete(tempDirectory, true);
                Directory.Delete(toRarDirecotry, true);
                return contents;
            }
        }

        private static Byte[] RARAttachmentData(Dictionary<String, Byte[]> attachments, String fileName)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var toRarDirecotry = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            Directory.CreateDirectory(toRarDirecotry);
            var rarFile = Path.Combine(tempDirectory, "Report" + ".rar");

            foreach (var attach in attachments)
            {
                if (attach.Value != null && attach.Value.Length > 0)
                {
                    var flyFile = Path.Combine(toRarDirecotry, attach.Key + ".xlsx");
                    File.WriteAllBytes(flyFile, attach.Value);
                }
            }

            String rarArguments = @"a -m5 -ep ""{0}"" ""{1}""";
            rarArguments = String.Format(
                rarArguments,
                rarFile,
                toRarDirecotry + "\\*"
            );

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = @"Rar.exe",
                Arguments = rarArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            });

            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception("Run WinRAR RAR Command Failed!, Exit Code: " + proc.ExitCode +
                    ", Details StandardOuput: " + proc.StandardOutput.ReadToEnd() +
                    ",StandardError" + proc.StandardError.ReadToEnd() +
                    ",StandardInput" + proc.StandardInput +
                    ",Site" + proc.Site);
            }
            else
            {
                var contents = File.ReadAllBytes(rarFile);
                Directory.Delete(tempDirectory, true);
                Directory.Delete(toRarDirecotry, true);
                return contents;
            }
        }

        private static Byte[] RARAttachmentDataNotExcel(Dictionary<String, Byte[]> attachments, String fileName)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var toRarDirecotry = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            Directory.CreateDirectory(toRarDirecotry);
            var rarFile = Path.Combine(tempDirectory, "Report" + ".rar");

            foreach (var attach in attachments)
            {
                if (attach.Value != null && attach.Value.Length > 0)
                {
                    var flyFile = Path.Combine(toRarDirecotry, attach.Key);
                    File.WriteAllBytes(flyFile, attach.Value);
                }
            }

            String rarArguments = @"a -m5 -ep ""{0}"" ""{1}""";
            rarArguments = String.Format(
                rarArguments,
                rarFile,
                toRarDirecotry + "\\*"
            );

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = @"Rar.exe",
                Arguments = rarArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            });

            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception("Run WinRAR RAR Command Failed!, Exit Code: " + proc.ExitCode +
                    ", Details StandardOuput: " + proc.StandardOutput.ReadToEnd() +
                    ",StandardError" + proc.StandardError.ReadToEnd() +
                    ",StandardInput" + proc.StandardInput +
                    ",Site" + proc.Site);
            }
            else
            {
                var contents = File.ReadAllBytes(rarFile);
                Directory.Delete(tempDirectory, true);
                Directory.Delete(toRarDirecotry, true);
                return contents;
            }
        }

        private static Byte[] ZipAttachmentData(Byte[] attachmentData, String subject)
        {
            var tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var tempZipDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            var flyFile = Path.Combine(tempDirectory, subject + ".xlsx");

            Directory.CreateDirectory(tempDirectory);
            Directory.CreateDirectory(tempZipDirectory);
            File.WriteAllBytes(flyFile, attachmentData);

            String zipFilePath = Path.Combine(tempZipDirectory, subject + ".zip");
            String zipArguments = String.Format(@"-9 -j ""{0}"" ""{1}""", zipFilePath, tempDirectory + "\\*");

            var proc = Process.Start(new ProcessStartInfo
            {
                FileName = "zip.exe",
                Arguments = zipArguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true
            });

            proc.WaitForExit();

            if (proc.ExitCode != 0)
            {
                throw new Exception("Run zip Command Failed!, Exit Code: " + proc.ExitCode +
                    ", Details StandardOuput: " + proc.StandardOutput.ReadToEnd() +
                    ",StandardError" + proc.StandardError.ReadToEnd() +
                    ",StandardInput" + proc.StandardInput +
                    ",Site" + proc.Site);
            }
            else
            {
                var contents = File.ReadAllBytes(zipFilePath);
                Directory.Delete(tempDirectory, true);
                Directory.Delete(tempZipDirectory, true);
                return contents;
            }
        }

    }
}

using Aspose.Email.Mail;
using Aspose.Pdf.Text;
using Aspose.Words;
using Aspose.Words.Saving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Loan.Core.Helper
{
    public class AsposeConvertHelper
    {
        public static byte[] ConvertWordToHtml(byte[] data)
        {
            Aspose.Words.Document document = new Aspose.Words.Document(new MemoryStream(data));
            HtmlSaveOptions options = new HtmlSaveOptions(SaveFormat.Html);
            options.ImageSavingCallback = new WordHandleImageSaving();
            MemoryStream result = new MemoryStream();
            document.Save(result, options);
            //        document.Save("d:\\asdf1.html", options);
            return result.GetBuffer();
        }

        public static void WordSaveasHtmlFile(byte[] data,string Target)
        {
            Aspose.Words.Document document = new Aspose.Words.Document(new System.IO.MemoryStream(data));
            document.Save(Target.ToString(), SaveFormat.Html);
        }

        public static void ExcelSaveasHtmlFile(byte[] data, string Target)
        {
            Aspose.Cells.Workbook document = new Aspose.Cells.Workbook();
            document.LoadData(new MemoryStream(data));
            document.Save(Target.ToString(), Aspose.Cells.FileFormatType.Html);  //SaveFormat.Html

      //      Aspose.Words.Document document = new Aspose.Words.Document(new System.IO.MemoryStream(data));
      //      document.Save(Target.ToString(), SaveFormat.Html);
        }

        public static string GetWordText(byte[] data)
        {
            var result = "";
            Aspose.Words.Document document = new Aspose.Words.Document(new MemoryStream(data));
            result = document.ToTxt();
            return result;
        }

        
        

        public static byte[] ConvertExcelToHtml(byte[] data)
        {
            Aspose.Cells.Workbook document = new Aspose.Cells.Workbook(new MemoryStream(data));
            Aspose.Cells.HtmlSaveOptions options = new Aspose.Cells.HtmlSaveOptions(Aspose.Cells.SaveFormat.Html);

            MemoryStream result = new MemoryStream();
            document.Save(result, options);
            document.Save("d:\\asdf1.xls", options);
            return result.GetBuffer();
        }

        public static byte[] ConvertExcelToHtmlByWgx(byte[] data,string strFilePath)
        {
            Aspose.Cells.Workbook document = new Aspose.Cells.Workbook(new MemoryStream(data));
            Aspose.Cells.HtmlSaveOptions options = new Aspose.Cells.HtmlSaveOptions(Aspose.Cells.SaveFormat.Html);

            MemoryStream result = new MemoryStream();
            document.Save(result, options);
            document.Save(strFilePath, options);
            return result.GetBuffer();
        }

        public static string GetExcelText(byte[] data)
        {
            var result = "";
            Aspose.Cells.Workbook document = new Aspose.Cells.Workbook(new MemoryStream(data));
            //result = document.
            for (var i = 0; i < document.Worksheets.Count; i++)
            {
                var cells = document.Worksheets[i].Cells;
                for (int k = 1; k < cells.MaxDataRow + 1; k++)
                {
                    for (int j = 0; j < cells.MaxDataColumn + 1; j++)
                    {
                        result += "\r\t" + cells[k, j].StringValue.Trim();
                    }
                }
            }
            return result;
        }

        public static string GetPdfText(byte[] data)
        {
            //open document
            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(new MemoryStream(data));

            //create TextAbsorber object to extract text
            TextAbsorber textAbsorber = new TextAbsorber();
           
            //accept the absorber for all the pages
            pdfDocument.Pages.Accept(textAbsorber);
         
            //get the extracted text
            return textAbsorber.Text;
        }

        public static string GetEmlText(byte[] data)
        {
            MailMessage msg = MailMessage.Load(new MemoryStream(data), MessageFormat.Eml);
            return msg.Body; //msg.HtmlBody;
        
        }

        public static string GetMhtText(byte[] data)
        {
            MailMessage msg = MailMessage.Load(new MemoryStream(data), MessageFormat.Mht);
            return msg.Body;  //msg.HtmlBody;
        }

        public static string GetMhtHtml(byte[] data)
        {
            MailMessage msg = MailMessage.Load(new MemoryStream(data), MessageFormat.Mht);
            return msg.HtmlBody;  //msg.HtmlBody;
        }

        public static string GetMsgText(byte[] data)
        {
            MailMessage msg = MailMessage.Load(new MemoryStream(data), MessageFormat.Msg);
            return msg.Body;  //msg.HtmlBody;
        }
    }

    public class WordHandleImageSaving : IImageSavingCallback
    {
        void IImageSavingCallback.ImageSaving(ImageSavingArgs e)
        {
            FileStream stream = new FileStream(System.AppDomain.CurrentDomain.BaseDirectory + "/rest/file/preview/" + e.ImageFileName, FileMode.CreateNew);
            e.ImageStream = stream;
            e.KeepImageStreamOpen = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loan.Data;
using Loan.Entity.DB;
using System.Net;
using System.IO;

namespace Loan.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create("http://su.bdimg.com/static/superplus/img/logo_white.png");
            Stream myStream = myRequest.GetResponse().GetResponseStream();
            FileStream fs = new FileStream("",FileMode.Create,FileAccess.ReadWrite);
            //定义一个字节数据
            byte[] btContent = new byte[512];
            int intSize = 0;
            intSize = myStream.Read(btContent, 0, 512);
            while (intSize > 0)
            {
                fs.Write(btContent, 0, intSize);
                intSize = myStream.Read(btContent, 0, 512);
            }
        }
    }
}

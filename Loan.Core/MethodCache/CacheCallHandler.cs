using System;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.Web;
using Common.Logging;

//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    public class CacheCallHandler : ICallHandler
    {
        private const int DefaultDurationMinutes = 30;

        //private string _cacheKey;
        private int _durationMinutes;
        private string cacheKey = "";
        //private DateTime start = DateTime.Now;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(CacheCallHandler));

        public CacheCallHandler(NameValueCollection attributes)
        {
            //_cacheKey = String.IsNullOrEmpty(attributes["CacheKey"]) ? string.Empty : attributes["CacheKey"];
            _durationMinutes = String.IsNullOrEmpty(attributes["DurationMinutes"]) ? DefaultDurationMinutes : int.Parse(attributes["DurationMinutes"]);
        }

        public void BeginInvoke(MethodContext context)
        {
            //start = DateTime.Now;
            cacheKey = JsonConvert.SerializeObject(context);
            //Console.WriteLine(cacheKey);

            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                context.ReturnValue = HttpRuntime.Cache.Get(cacheKey);
                context.Processed = true;
            }

            //Console.WriteLine("BeginInvoke");
        }

        public void EndInvoke(MethodContext context)
        {

            if (context.ReturnValue != null)
            {
                if (HttpRuntime.Cache.Get(cacheKey) == null)
                {
                    lock (cacheKey)
                    {
                        DateTime expiredTime = DateTime.Now.AddMinutes(_durationMinutes);
                        HttpRuntime.Cache.Add(cacheKey, context.ReturnValue, null, expiredTime, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.Normal, null);
                    }
                }
            }
            //_logger.Info(context.ClassName + context.MethodName + "花费时间：" + (DateTime.Now - start));
            //Console.WriteLine("EndInvoke");
        }

        public void OnException(MethodContext context)
        {
            //Console.WriteLine(context.Exception.Message);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Loan.Core.Attributes
{
    public class StringWriterWithEncoding : StringWriter
    {
        Encoding encoding;
        public StringWriterWithEncoding(Encoding encoding)
        {
            this.encoding = encoding;
        }
        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }

    public class ActionOutputCacheAttribute : ActionFilterAttribute
    {
        private static MethodInfo _switchWriterMethod = typeof(HttpResponse).GetMethod("SwitchWriter", BindingFlags.Instance | BindingFlags.NonPublic);

        public ActionOutputCacheAttribute(int cacheDuration)
        {
            _cacheDuration = cacheDuration;
        }

        //目前还不能设置为Client缓存，会与OutputCache同样的问题
        private CachePolicy _cachePolicy = CachePolicy.Server;
        private int _cacheDuration;
        private TextWriter _originalWriter;
        private string _cacheKey;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Server-side caching?
            if (_cachePolicy == CachePolicy.Server || _cachePolicy == CachePolicy.ClientAndServer)
            {
                _cacheKey = GenerateCacheKey(filterContext);
                CacheContainer cachedOutput = (CacheContainer)filterContext.HttpContext.Cache[_cacheKey];
                if (cachedOutput != null)
                {
                    filterContext.HttpContext.Response.ContentType = cachedOutput.ContentType;
                    filterContext.Result = new ContentResult { Content = cachedOutput.Output };
                }
                else
                {
                    StringWriter stringWriter = new StringWriterWithEncoding(filterContext.HttpContext.Response.ContentEncoding);
                    HtmlTextWriter newWriter = new HtmlTextWriter(stringWriter);
                    _originalWriter = (TextWriter)_switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { newWriter });
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Server-side caching?
            if (_cachePolicy == CachePolicy.Server || _cachePolicy == CachePolicy.ClientAndServer)
            {
                if (_originalWriter != null) // Must complete the caching
                {
                    HtmlTextWriter cacheWriter = (HtmlTextWriter)_switchWriterMethod.Invoke(HttpContext.Current.Response, new object[] { _originalWriter });
                    string textWritten = ((StringWriter)cacheWriter.InnerWriter).ToString();
                    filterContext.HttpContext.Response.Write(textWritten);
                    CacheContainer container = new CacheContainer(textWritten, filterContext.HttpContext.Response.ContentType);
                    filterContext.HttpContext.Cache.Add(_cacheKey, container, null, DateTime.Now.AddSeconds(_cacheDuration), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                }
            }
        }

        private string GenerateCacheKey(ActionExecutingContext filterContext)
        {
            StringBuilder cacheKey = new StringBuilder("OutputCacheKey:");

            // Controller + action
            cacheKey.Append(filterContext.Controller.GetType().FullName.GetHashCode());
            if (filterContext.RouteData.Values.ContainsKey("action"))
            {
                cacheKey.Append("_");
                cacheKey.Append(filterContext.RouteData.Values["action"].ToString());
            }

            foreach (KeyValuePair<string, object> pair in filterContext.ActionParameters)
            {
                cacheKey.Append("_");
                cacheKey.Append(pair.Key);
                cacheKey.Append("=");

                if (pair.Value != null)
                    cacheKey.Append(pair.Value.ToString());
                else
                    cacheKey.Append(string.Empty);
            }

            return cacheKey.ToString();
        }

        private class CacheContainer
        {
            public string Output;
            public string ContentType;
            public CacheContainer(string data, string contentType)
            {
                Output = data;
                ContentType = contentType;
            }
        }

        public enum CachePolicy
        {
            NoCache = 0,
            Client = 1,
            Server = 2,
            ClientAndServer = 3
        }
    }
}

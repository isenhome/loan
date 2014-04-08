using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loan.Core.Extension
{
    public static class RequestQueryStringExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        public static List<T> GetRequestParamList<T>(this HttpRequestBase request, string keyPrefix)
        {
            return (from k in request.Params.AllKeys
                    where k.StartsWith(keyPrefix)
                    orderby k
                    select (T)Convert.ChangeType(request.Params[k], typeof(T))
                   ).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="keyPrefix"></param>
        /// <returns></returns>
        public static T GetRequestParam<T>(this HttpRequestBase request, string keyPrefix)
        {
            if (request.QueryString[keyPrefix] == null)
                return default(T);
            return (T)Convert.ChangeType(request.QueryString[keyPrefix], typeof(T));
        }
    }
}

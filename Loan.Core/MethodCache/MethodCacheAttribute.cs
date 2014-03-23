using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;

//namespace ConsoleApplicationDemo.EmitAOP
namespace Loan.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MethodCacheAttribute : AspectAttribute
    {
        private readonly static PropertyInfo[] Properties = typeof(MethodCacheAttribute).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        public MethodCacheAttribute()
        {
            CallHandlerType = typeof(CacheCallHandler);
        }

        //public string CacheKey { get; set; }
        public string Description { get; set; }

        public int DurationMinutes { get; set; }

        protected override PropertyInfo[] PropertiesInfo
        {
            get { return Properties; }
        }
    }
}

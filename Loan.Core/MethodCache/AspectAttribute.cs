using System;
using System.Collections.Specialized;
using System.Reflection;

namespace Loan.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class AspectAttribute : Attribute
    {
        public Type CallHandlerType { get; protected set; }

        protected abstract PropertyInfo[] PropertiesInfo { get; }

        public NameValueCollection GetAttrs()
        {
            NameValueCollection attrs = new NameValueCollection();

            foreach (var p in PropertiesInfo)
            {
                attrs.Add(p.Name, p.GetValue(this, null).ToString());
            }

            return attrs;
        }
    }
}
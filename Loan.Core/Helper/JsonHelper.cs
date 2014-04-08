using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Loan.Core.Helper.Helper;
using System.Dynamic;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;

namespace Loan.Core.Helper
{
    public class JsonHelper
    {
        // Json 序列化  
        public static string ToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }

        //反序列化  
        public static T FromJsonTo<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                T jsonObject = (T)ser.ReadObject(ms);
                return jsonObject;
            }
        }

        /// <summary>
        /// 从json字符串到对象。
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static dynamic FormatJson(string jsonStr)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            jss.RegisterConverters(new JavaScriptConverter[] { new DynamicJsonConverter() });
            dynamic glossaryEntry = jss.Deserialize(jsonStr, typeof(object)) as dynamic;
            return glossaryEntry;
        }

        public static bool IsExist(dynamic obj, string key)
        {
            var list = obj.GetDynamicMemberNames();
            foreach (var item in list)
            {
                if (item == key)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class DynamicJsonConverter : JavaScriptConverter
    {
        public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            if (type == typeof(object))
            {
                return new DynamicJsonObject(dictionary);
            }

            return null;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(object) })); }
        }
    }
    public class DynamicJsonObject : DynamicObject
    {
        private IDictionary<string, object> Dictionary { get; set; }

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            this.Dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (!this.Dictionary.ContainsKey(binder.Name))
            {
                Dictionary.Add(binder.Name, null);
                //return false;
            }
            result = this.Dictionary[binder.Name];
            if (result is IDictionary<string, object>)
            {
                result = new DynamicJsonObject(result as IDictionary<string, object>);
            }
            else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
            {
                result = new List<DynamicJsonObject>((result as ArrayList).ToArray().Select(x => new DynamicJsonObject(x as IDictionary<string, object>)));
            }
            else if (result is ArrayList)
            {
                var temp = (result as ArrayList);
                if (temp.Count > 0 && (temp[0] is IDictionary<string, object>))
                {
                    result = new List<DynamicJsonObject>((result as ArrayList).ToArray().Select(x => new DynamicJsonObject(x as IDictionary<string, object>)));
                }
                else
                {
                    result = new List<object>((result as ArrayList).ToArray());
                }
            }

            return this.Dictionary.ContainsKey(binder.Name);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.Dictionary.Keys;
        }
    }
}

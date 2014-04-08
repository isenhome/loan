using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Loan.Core.Attributes;

namespace Loan.Core.Helper
{
    public static class EnumHelper
    {
        /// <summary>
        /// 取得枚举列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetEnumList<T>(Enum en = null)
        {
            var t = typeof(T);
            Array a = Enum.GetValues(t);
            for (int i = 0; i < a.Length; i++)
            {
                SelectListItem item = new SelectListItem();
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                if (en != null)
                {
                    item.Selected = (enumKey == en.GetHashCode());
                }
                item.Text = enumDescription;
                item.Value = enumKey.ToString();
                yield return item;
            }
        }


        /// <summary>
        /// 取得有排除项的枚举列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetExpEnumList<T>(List<Enum> en = null)
        {
            var t = typeof(T);
            Array a = Enum.GetValues(t);

            for (int i = 0; i < a.Length; i++)
            {
                bool flag = true;
                SelectListItem item = new SelectListItem();
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                if (en != null)
                {
                    foreach (var enitem in en)
                    {
                        if (enumKey == enitem.GetHashCode())
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag == true)
                {
                    item.Text = enumDescription;
                    item.Value = enumKey.ToString();
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 取得有排除项的枚举列表,并设置选中项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetExpEnumList<T>(List<Enum> en, Enum enSelected)
        {
            var t = typeof(T);
            Array a = Enum.GetValues(t);

            for (int i = 0; i < a.Length; i++)
            {
                bool flag = true;
                SelectListItem item = new SelectListItem();
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                if (en != null)
                {
                    foreach (var enitem in en)
                    {
                        if (enumKey == enitem.GetHashCode())
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag == true)
                {
                    item.Text = enumDescription;
                    item.Value = enumKey.ToString();
                    if (enSelected != null)
                    {
                        item.Selected = (enumKey == enSelected.GetHashCode());
                    }
                    yield return item;
                }
            }
        }

        public static Dictionary<Int32, String> GetEnumDict<T>()
        {
            Dictionary<Int32, String> dict = new Dictionary<Int32, String>();
            IEnumerable<SelectListItem> result = GetEnumList<T>();

            if (result != null && result.Count() > 0)
            {
                foreach (var item in result)
                {
                    dict.Add(Int32.Parse(item.Value), item.Text);
                }
            }

            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj)
        {
            try
            {
                var t = obj.GetType();
                var v = obj.GetHashCode();
                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch (Exception ex)
            {
                return "UNKNOWN";
            }
        }

        public static Int32 GetMapValue(this Enum obj)
        {
            try
            {
                var t = obj.GetType();
                var v = obj.GetHashCode();
                FieldInfo fi = t.GetField(GetName(t, v));
                MapValueAttribute[] attributes = (MapValueAttribute[])fi.GetCustomAttributes(typeof(MapValueAttribute), false);
                return (attributes.Length > 0) ? attributes[0].MapValue : -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public static string GetMapType(this Enum obj)
        {
            try
            {
                var t = obj.GetType();
                var v = obj.GetHashCode();
                FieldInfo fi = t.GetField(GetName(t, v));
                MapTypeAttribute[] attributes = (MapTypeAttribute[])fi.GetCustomAttributes(typeof(MapValueAttribute), false);
                return (attributes.Length > 0) ? attributes[0].MapType : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static String GetFieldName(this Enum obj)
        {
            try
            {
                var t = obj.GetType();
                var v = obj.GetHashCode();
                FieldInfo fi = t.GetField(GetName(t, v));
                FieldNameAttribute[] attributes = (FieldNameAttribute[])fi.GetCustomAttributes(typeof(FieldNameAttribute), false);
                return (attributes.Length > 0) ? attributes[0].FieldName : GetName(t, v);
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private static string GetDescription(System.Type t, object v)
        {
            try
            {
                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch (Exception ex)
            {
                return "UNKNOWN";
            }
        }
        private static string GetName(System.Type t, object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch (Exception ex)
            {
                return "UNKNOWN";
            }
        }


    }
}

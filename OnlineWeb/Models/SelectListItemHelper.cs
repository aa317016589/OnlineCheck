using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace OnlineCheck.Web.Models
{
    public static class SelectListItemHelper
    {
        public static IEnumerable<SelectListItem> FindByEnum<T>(Int32? def = null, Boolean pleaseCheck = true, IEnumerable<T> ignores = null, IEnumerable<T> enums = null) where T : struct
        {
            Type type = typeof(T);

            if (!type.IsEnum)
            {
                throw new Exception();
            }

            if (enums == null || !enums.Any())
            {
                enums = Enum.GetValues(typeof(T)).Cast<T>();
            }

            if (ignores != null)
            {
                enums = enums.Where(s => !ignores.Contains(s));
            }

            List<SelectListItem> vat = new List<SelectListItem>();

            foreach (T item in enums)
            {
                Enum en = (Enum)Enum.Parse(type, item.ToString());

                Int32 v = Convert.ToInt32(en);

                Boolean selected = def.HasValue ? def == v : false;

                vat.Add(new SelectListItem() { Text = en.GetDescription(), Value = v.ToString(), Selected = selected });
            }

            if (pleaseCheck)
            {
                vat.Insert(0, new SelectListItem() { Text = "全部", Value = "" });
            }

            return vat;
        }  
    }

    public static class EnumHelper
    {
        /// <summary>
        ///  获取枚举类型上面的描述
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum obj)
        {
            return GetDescriptions(obj).LastOrDefault();
        }

        public static IList<String> GetDescriptions(this Enum obj)
        {
            if (obj == null)
            {
                return new List<String>();
            }

            String[] flags = new Regex(", ").Split(obj.ToString());

            Type t = obj.GetType();

            List<String> descriptions = new List<string>(flags.Length);

            flags.ForEach(s =>
            {
                FieldInfo fi = t.GetField(s);

                DescriptionAttribute[] arrDesc =
                    (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);


                descriptions.Add(arrDesc.Length == 1 ? arrDesc[0].Description : "");

            });

            return descriptions;
        }

        public static IEnumerable<T> GetEnums<T>(this Enum obj)
        {
            String[] flags = new Regex(", ").Split(obj.ToString());

            Type t = typeof(T);

            return flags.Select(s => (T)Enum.Parse(t, s));
        }

        public static IEnumerable<T> GetValues<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerables, Action<T> action)
        {
            foreach (var enumerable in enumerables)
            {
                action(enumerable);
            }
        }
    }
}
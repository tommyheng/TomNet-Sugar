using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using TomNet.Extensions;


namespace TomNet.AspNetCore.Mvc
{
    /// <summary>
    /// MVC相关扩展方法
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// 判断类型是否是Controller
        /// </summary>
        public static bool IsController(this Type type)
        {
            return IsController(type.GetTypeInfo());
        }

        /// <summary>
        /// 判断类型是否是Controller
        /// </summary>
        public static bool IsController(this TypeInfo typeInfo)
        {
            return typeInfo.IsClass && !typeInfo.IsAbstract && typeInfo.IsPublic && !typeInfo.ContainsGenericParameters
                && !typeInfo.IsDefined(typeof(NonControllerAttribute)) && (typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)
                    || typeInfo.IsDefined(typeof(ControllerAttribute)));
        }

        /// <summary>
        /// 获取Area名
        /// </summary>
        public static string GetAreaName(this ActionContext context)
        {
            string area = null;
            if (context.RouteData.Values.TryGetValue("area", out object value))
            {
                area = (string)value;
                if (area.IsNullOrWhiteSpace())
                {
                    area = null;
                }
            }
            return area;
        }

        /// <summary>
        /// 获取Controller名
        /// </summary>
        public static string GetControllerName(this ActionContext context)
        {
            return context.RouteData.Values["controller"].ToString();
        }

        /// <summary>
        /// 获取Action名
        /// </summary>
        public static string GetActionName(this ActionContext context)
        {
            return context.RouteData.Values["action"].ToString();
        }
    }
}
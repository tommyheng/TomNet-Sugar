﻿using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using TomNet.Core;
using TomNet.Exceptions;
using TomNet.Reflection;
using TomNet.SqlSugarCore.Entity;

namespace TomNet.AspNetCore.Mvc
{
    /// <summary>
    /// MVC 功能处理器
    /// </summary>
    public class MvcFunctionHandler : FunctionHandlerBase
    {
        /// <summary>
        /// 初始化一个<see cref="FunctionHandlerBase"/>类型的新实例
        /// </summary>
        public MvcFunctionHandler(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            FunctionTypeFinder = serviceProvider.GetService<IFunctionTypeFinder>();
            MethodInfoFinder = new MvcMethodInfoFinder();
        }

        /// <summary>
        /// 获取 功能类型查找器
        /// </summary>
        public override IFunctionTypeFinder FunctionTypeFinder { get; }

        /// <summary>
        /// 获取 功能方法查找器
        /// </summary>
        public override IMethodInfoFinder MethodInfoFinder { get; }

        /// <summary>
        /// 重写以实现从功能类型创建功能信息
        /// </summary>
        /// <param name="controllerType">功能类型</param>
        /// <returns></returns>
        protected override Function GetFunction(Type controllerType)
        {
            if (!controllerType.IsController())
            {
                throw new TomNetException($"类型“{controllerType.FullName}”不是MVC控制器类型");
            }
            FunctionAccessType accessType = controllerType.HasAttribute<LoggedInAttribute>() || controllerType.HasAttribute<AuthorizeAttribute>()
                ? FunctionAccessType.LoggedIn
                : controllerType.HasAttribute<RoleLimitAttribute>()
                    ? FunctionAccessType.RoleLimit
                    : FunctionAccessType.Anonymous;

            bool isApi = controllerType.HasAttribute<ApiControllerAttribute>();
            var source = options.LocalOption.AppKey;
            Function function = new Function()
            {
                Source = source,
                IsWebApi = isApi,
                Name = controllerType.GetDescription(),
                Area = GetArea(controllerType),
                Controller = controllerType.Name.Replace("ControllerBase", string.Empty).Replace("Controller", string.Empty),
                IsController = true,
                AccessType = accessType
            };
            return function;
        }

        /// <summary>
        /// 重写以实现从方法信息中创建功能信息
        /// </summary>
        /// <param name="typeFunction">类功能信息</param>
        /// <param name="method">方法信息</param>
        /// <returns></returns>
        protected override Function GetFunction(Function typeFunction, MethodInfo method)
        {
            FunctionAccessType accessType = method.HasAttribute<LoggedInAttribute>() || method.HasAttribute<AuthorizeAttribute>()
                ? FunctionAccessType.LoggedIn
                : method.HasAttribute<AllowAnonymousAttribute>()
                    ? FunctionAccessType.Anonymous
                    : method.HasAttribute<RoleLimitAttribute>()
                        ? FunctionAccessType.RoleLimit
                        : typeFunction.AccessType;

            var source = options.LocalOption.AppKey;
            Function function = new Function()
            {
                Source = source,
                IsWebApi = typeFunction.IsWebApi,
                Name = $"{typeFunction.Name}-{method.GetDescription()}",
                Area = typeFunction.Area,
                Controller = typeFunction.Controller,
                Action = method.Name,
                AccessType = accessType,
                IsController = false,
            };
            return function;
        }

        ///// <summary>
        ///// 重写以实现是否忽略指定方法的功能信息
        ///// </summary>
        ///// <param name="action">要判断的功能信息</param>
        ///// <param name="method">功能相关的方法信息</param>
        ///// <param name="functions">已存在的功能信息集合</param>
        ///// <returns></returns>
        //protected override bool IsIgnoreMethod(Function action, MethodInfo method, IEnumerable<Function> functions)
        //{
        //    bool flag = base.IsIgnoreMethod(action, method, functions);
        //    return flag && method.HasAttribute<HttpPostAttribute>() || method.HasAttribute<NonActionAttribute>();
        //}

        /// <summary>
        /// 从类型中获取功能的区域信息
        /// </summary>
        private static string GetArea(Type type)
        {
            AreaAttribute attribute = type.GetAttribute<AreaAttribute>();
            return attribute?.RouteValue;
        }
    }
}
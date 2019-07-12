using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using TomNet.App.Model.DTO;
using TomNet.AspNetCore;
using TomNet.AspNetCore.Data;
using TomNet.Data;
using TomNet.Extensions;
using TomNet.Security.Claims;
using TomNet.Core;
using TomNet.App.Core.Contracts.Security;

namespace TomNet.App.WebApi.Filters
{
    /// <summary>
    /// 功能权限授权验证
    /// </summary>
    public class FunctionAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //允许匿名访问
            if (context.Filters.Any(m => m is IAllowAnonymousFilter))
            {
                return;
            }

            Check.NotNull(context, nameof(context));
            IPrincipal user = context.HttpContext.User;
            //未登录
            if (user == null || !user.Identity.IsAuthenticated)
            {
                return;
            }

            bool isJsRequest = context.HttpContext.Request.IsAjaxRequest() || context.HttpContext.Request.IsJsonContextType();
            //已登录
            if (user.Identity.IsAuthenticated)
            {
                var roleId = user.Identity.GetClaimValueFirstOrDefault("RoleId");
                var role = user.Identity.GetClaimValueFirstOrDefault(ClaimTypes.Role);
                bool.TryParse(user.Identity.GetClaimValueFirstOrDefault("UserLocked"), out bool userLocked);
                bool.TryParse(user.Identity.GetClaimValueFirstOrDefault("RoleLocked"), out bool roleLocked);


                if (userLocked || roleLocked)
                {
                    //进入用户或角色锁定界面或者返回被锁定数据
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult("权限被锁定，无法访问", AjaxResultType.Locked))
                        : new RedirectResult("/Exception/Locked");
                }

                if (role == "超级管理员")
                {
                    return;
                }

                IServiceProvider provider = context.HttpContext.RequestServices;
                var service = provider.GetService<IRoleFunctionMapContract>();
                var options = provider.GetTomNetOptions();

                string area = GetAreaName(context);
                string controller = GetControllerName(context);
                string action = GetActionName(context);
                var dto = new RoleFunctionMapDto
                {
                    RoleId = int.Parse(roleId),
                    Source = options.LocalOption.AppKey,
                    Area = area,
                    Controller = controller,
                    Action = action
                };


                //判断是否基于角色权限授权
                if (!service.Authorize(dto))
                {
                    context.Result = isJsRequest
                        ? (IActionResult)new JsonResult(new AjaxResult("权限不足，无法访问", AjaxResultType.Forbidden))
                        : new RedirectResult("/Exception/Forbidden");
                }

            }

            if (isJsRequest)
            {
                context.HttpContext.Response.StatusCode = 200;
            }
        }


        /// <summary>
        /// 获取Area名
        /// </summary>
        public string GetAreaName(ActionContext context)
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
        public string GetControllerName(ActionContext context)
        {
            return context.RouteData.Values["controller"].ToString();
        }

        /// <summary>
        /// 获取Action名
        /// </summary>
        public string GetActionName(ActionContext context)
        {
            return context.RouteData.Values["action"].ToString();
        }
    }
}

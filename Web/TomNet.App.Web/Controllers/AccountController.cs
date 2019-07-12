using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomNet.App.Core.Contracts.Identity;
using TomNet.App.Model.DB.Identity;
using TomNet.App.Model.DTO;
using TomNet.AspNetCore.Data;
using TomNet.Data;
using TomNet.SqlSugarCore.Entity;

using Microsoft.Extensions.DependencyInjection;


namespace TomNet.App.Web.Controllers
{
    [Description("账号管理")]
    public class AccountController : Controller
    {
        private readonly IUserInfoContract _userInfoContract;
        private readonly IUserLoginContract _userLoginContract;
        public AccountController(IServiceProvider serviceProvider,
            IUserLoginContract userLoginContract,
            IUserInfoContract userInfoContract)
        {
            IDbFactory db = serviceProvider.GetService<IDbFactory>();

      
            _userInfoContract = userInfoContract;
            _userLoginContract = userLoginContract;
        }

        [HttpGet]
        [AllowAnonymous]
        [Description("登录")]
        public IActionResult SignIn(string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Description("登录")]
        public async Task<AjaxResult> SignIn(LoginDto entity)
        {
            if (ModelState.IsValid)
            {
                var user = _userLoginContract.UnitOfWork.DbContext.Queryable<UserRoleMap, UserLogin, UserInfo, Role>((urm, ul, ui, r)
                     => urm.UserId == ul.Id && urm.RoleId == r.Id && urm.UserId == ui.UserId)
                     .Where((urm, ul, ui, r) => ul.UserName == entity.UserName && ul.Password == entity.Password)
                     .Select((urm, ul, ui, r) => new { ul.Id, ul.UserName, RoleName = r.Name, ui.NickName, urm.RoleId, UserLocked = ul.IsLocked, RoleLocked = r.IsLocked }).First();


                bool succee = user != null;

                if (succee)
                {
                    //创建用户身份标识
                    var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaims(new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, user.RoleName),
                        new Claim(ClaimTypes.GivenName, user.NickName),
                        new Claim("RoleId", user.RoleId.ToString()),
                        new Claim("UserLocked", user.UserLocked.ToString()),
                        new Claim("RoleLocked", user.RoleLocked.ToString())
                    });

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            //此处将会覆盖CookieAuthenticationOptions处的设置
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                            AllowRefresh = true
                        });
                    return new AjaxResult("登录成功");
                }
                else
                {
                    return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
                }
            }

            return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
        }

        [Description("注销")]
        [AllowAnonymous]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Default/Index");
        }

    }
}
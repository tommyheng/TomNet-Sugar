using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TomNet.AspNetCore.Data;

namespace TomNet.App.Web.Controllers
{
    [Description("异常提示")]
    public class ExceptionController : Controller
    {
        [AllowAnonymous]
        [Description("系统异常")]
        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        [Description("用户未登录")]
        public IActionResult UnAuth()
        {
            return View();
        }

        [AllowAnonymous]
        [Description("权限不足")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [AllowAnonymous]
        [Description("资源未找到")]
        public IActionResult NoFound()
        {
            return View();
        }

        [AllowAnonymous]
        [Description("资源被锁定")]
        public IActionResult Locked()
        {
            return View();
        }
    }
}
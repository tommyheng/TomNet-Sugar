using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TomNet.App.Model.DB.Security;
using TomNet.Core;
using TomNet.SqlSugarCore.Entity;


namespace TomNet.App.Web.Controllers
{
    [Authorize]
    [Description("管理后台")]
    public class DefaultController : Controller
    {
        protected ILogger Logger => HttpContext.RequestServices.GetLogger(GetType());

        public DefaultController( )
        {

        }

        [Description("后台首页")]
        public IActionResult Index()
        {

            return View();
        }

        [Description("默认页")]
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
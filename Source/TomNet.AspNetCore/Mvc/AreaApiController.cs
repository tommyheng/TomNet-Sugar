using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TomNet.Core;

namespace TomNet.AspNetCore.Mvc
{
    /// <summary>
    /// WebApi的区域控制器基类
    /// </summary>
    [ApiController]
    [Route("api/[area]/[controller]/[action]")]
    public abstract class AreaApiController : Controller
    {
        /// <summary>
        /// 获取或设置 日志对象
        /// </summary>
        protected ILogger Logger => HttpContext.RequestServices.GetLogger(GetType());
    }
}
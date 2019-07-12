using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using TomNet.App.Core.Contracts.Identity;
using TomNet.App.Model.DTO;
using TomNet.AspNetCore.Data;
using TomNet.AspNetCore.Mvc;
using TomNet.Authentication.JwtBearer;
using TomNet.Core;
using TomNet.Core.Options;
using TomNet.Data;

namespace TomNet.App.WebApi.Controllers
{
    [Authorize]
    public class IdentityController : ApiController
    {
        IDistributedCache _cache;
        IUserLoginContract _userLoginContract;

        public IdentityController(IUserLoginContract userLoginContract, IDistributedCache cache)
        {
            _cache = cache;
            _userLoginContract = userLoginContract;
        }

        /// <summary>
        /// Jwt登录
        /// </summary>
        /// <returns>JSON操作结果</returns>
        [AllowAnonymous]
        [HttpPost]
        public AjaxResult Jwtoken(LoginDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }

            var user = _userLoginContract.GetFirst(m => m.UserName == dto.UserName && m.Password == dto.Password);

            if (user == null)
            {
                return new AjaxResult("账户或密码错误", AjaxResultType.Error);
            }
            //生成Token，这里只包含最基本信息，其他信息从在线用户缓存中获取
            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            TomNetOptions options = HttpContext.RequestServices.GetTomNetOptions();
            string token = JwtHelper.CreateToken(claims, options);

            return new AjaxResult("登录成功", AjaxResultType.Success, token);
        }

        public AjaxResult TestApi()
        {
            var y = this.User.Identity.Name;
            return new AjaxResult("允许访问", AjaxResultType.Success, y);
        }

        [AllowAnonymous]
        public AjaxResult FreeApi()
        {
            var user = _userLoginContract.Entities.First();
            return new AjaxResult("允许访问", AjaxResultType.Success, user);
        }
    }
}
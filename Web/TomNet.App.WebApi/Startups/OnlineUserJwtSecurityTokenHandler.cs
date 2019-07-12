using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.IdentityModel.Tokens;

using TomNet.Collections;
using TomNet.Authentication;
using TomNet.Dependency;

namespace TomNet.App.WebApi.Startups
{
    public class OnlineUserJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        protected override ClaimsIdentity CreateClaimsIdentity(JwtSecurityToken jwtToken, string issuer, TokenValidationParameters validationParameters)
        {
            ClaimsIdentity identity = base.CreateClaimsIdentity(jwtToken, issuer, validationParameters);

            if (identity.IsAuthenticated)
            {
                //由在线缓存获取用户信息赋给IIdentity
                IOnlineUserCache onlineUserCache = ServiceLocator.Instance.GetService<IOnlineUserCache>();
                OnlineUser user = onlineUserCache.GetOrRefresh(identity.Name);
                if (user == null)
                {
                    return null;
                }
                // ============================= 此处附加用户信息 =============================
                //if (!string.IsNullOrEmpty(user.NickName))
                //{
                //    identity.AddClaim(new Claim(ClaimTypes.GivenName, user.NickName));
                //}
                //if (!string.IsNullOrEmpty(user.Email))
                //{
                //    identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                //}

                if (user.Roles.Length > 0)
                {
                    //identity.AddClaim(new Claim(ClaimTypes.Role, user.Roles.ExpandAndToString()));
                    identity.AddClaim(new Claim("RoleId", user.Roles.ExpandAndToString()));
                }

                //identity.AddClaim(new Claim("test", "自定义的数据"));

            }


            return identity;
        }
    }
}
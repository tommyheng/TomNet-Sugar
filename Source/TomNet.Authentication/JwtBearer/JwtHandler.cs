using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using TomNet.Core.Options;
using TomNet.Exceptions;


namespace TomNet.Authentication.JwtBearer
{
    /// <summary>
    /// Jwt辅助操作类
    /// </summary>
    public class JwtHelper
    {
        /// <summary>
        /// 生成JwtToken
        /// </summary>
        public static string CreateToken(Claim[] claims, TomNetOptions options)
        {
            JwtOptions jwtOptions = options.Jwt;
            string secret = jwtOptions.Secret;
            if (secret == null)
            {
                throw new TomNetException("创建JwtToken时Secret为空");
            }
            SecurityKey key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            DateTime now = DateTime.Now;
            double days = Math.Abs(jwtOptions.ExpireDays) > 0 ? Math.Abs(jwtOptions.ExpireDays) : 7;
            DateTime expires = now.AddDays(days);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                SigningCredentials = credentials,
                NotBefore = now,
                IssuedAt = now,
                Expires = expires
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
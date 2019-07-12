using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TomNet.App.Core.Contracts.Identity;
using TomNet.App.Model.DB.Identity;
using TomNet.Authentication;

namespace TomNet.App.WebApi.Startups
{
    /// <summary>
    /// 在线用户信息提供者
    /// </summary>
    public class OnlineUserProvider : IOnlineUserProvider
    {
        /// <summary>
        /// 创建在线用户信息
        /// </summary>
        /// <param name="provider">服务提供器</param>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        public virtual async Task<OnlineUser> CreateAsync(IServiceProvider provider, string userName)
        {
            // ============================= 此处附加用户信息 =============================
            IUserLoginContract userContract = provider.GetService<IUserLoginContract>();

            var user = await userContract.UnitOfWork.DbContext
                .Queryable<UserRoleMap, UserLogin, Role>((ur, u, r) => ur.UserId == u.Id && ur.RoleId == r.Id)
                .Where((ur, u, r) => u.UserName == userName)
                .Select((ur, u, r) => new { u.Id, u.UserName, RoleId = r.Id }).FirstAsync();
            ;
            if (user == null)
            {
                return null;
            }

            return new OnlineUser()
            {
                Id = user.Id.ToString(),
                UserName = user.UserName,
                Roles = new string[] { user.RoleId.ToString() }
            };
        }
    }
}
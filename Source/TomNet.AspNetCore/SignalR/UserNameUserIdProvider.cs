using System.Security.Claims;

using Microsoft.AspNetCore.SignalR;


namespace TomNet.AspNetCore.SignalR
{
    /// <summary>
    /// 用户名用户标识提供者
    /// </summary>
    public class UserNameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }
    }
}
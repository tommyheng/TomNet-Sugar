using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using TomNet.App.Core.Contracts.Identity;

namespace TomNet.App.Web.Hubs
{

    public class ChatHub : Hub
    {
        IDistributedCache _cache;
        IUserLoginContract _userLoginContract;

        public ChatHub(IUserLoginContract userLoginContract, IDistributedCache cache)
        {
            _cache = cache;
            _userLoginContract = userLoginContract;
        }

        /// <summary>
        /// 建立连接时触发
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Connected", $"{Context.ConnectionId} joined");
        }

        /// <summary>
        /// 离开连接时触发
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {

            await Clients.All.SendAsync("Disconnected", $"{Context.ConnectionId} left");
        }

        public async Task SendMessage(string user, string message)
        {
            var t = Context.User.Identity.IsAuthenticated;
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
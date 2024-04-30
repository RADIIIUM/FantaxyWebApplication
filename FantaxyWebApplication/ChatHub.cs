using FantaxyWebApplication.Models;
using Microsoft.AspNetCore.SignalR;

namespace FantaxyWebApplication
{
    public class ChatHub : Hub
    {
        public async Task Send(MessageModel message)
        {
            await this.Clients.All.SendAsync("Receive", message);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using QuickPickSignlaRApi.Models;

namespace QuickPickSignlaRApi.Hubs
{
    public class StatusHub : Hub
    {
        public async Task SendStatusAsync(Order client)
        {
            if(!string.IsNullOrEmpty(client.OrderedBy))
                await Clients.User(client.OrderedBy).SendAsync("MobileStatusReceived",client.Status);
        }
        public async Task SendStatusToKiosAsync(string status)
        {
                await Clients.All.SendAsync("DesktopStatusReceived", status);
        }
    }
}

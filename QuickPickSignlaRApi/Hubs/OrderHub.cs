using Microsoft.AspNetCore.SignalR;
using QuickPickSignlaRApi.Models;

namespace QuickPickSignlaRApi.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderAsync(Order order)
        {
            await Clients.All.SendAsync("OrderReceived", order);
        }
        public async Task SendReadyOrderAsync(Order order)
        {
            await Clients.All.SendAsync("ReadyOrderReceived", order);
        }
        public async Task SendChangedOrderAsync(Order order)
        {
            await Clients.All.SendAsync("OrderChangeded", order);
        }
        public async Task SendDeletedOrderAsync(Order order)
        {
            await Clients.All.SendAsync("OrderDeleted", order);
        }
    }
}

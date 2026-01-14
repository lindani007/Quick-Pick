using Microsoft.AspNetCore.SignalR;
using QuickPickSignlaRApi.Models;

namespace QuickPickSignlaRApi.Hubs
{
    public class OrderedItemHub : Hub
    {
        public async Task SendItemsAsync(List<BoughtItem> items)
        {
            await Clients.All.SendAsync("ItemReceived", items);
        }
        public async Task SendChangedItemsAsync(List<BoughtItem> items)
        {
            await Clients.All.SendAsync("ItemChanged", items);
        }
    }
}

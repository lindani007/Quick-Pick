using Microsoft.AspNetCore.SignalR;
using QuickPickSignlaRApi.Models;

namespace QuickPickSignlaRApi.Hubs
{
    public class ItemHub : Hub
    {
        public async Task SendItemAsync(Item item)
        {
            await Clients.All.SendAsync("ItemReceived", item);
        }
        public async Task SendUpdatedItemAsync(Item item)
        {
            await Clients.All.SendAsync("ItemUpDated", item);
        }
        public async Task SendDeletedItemAsync(Item item)
        {
            await Clients.All.SendAsync("ItemDeleted", item);
        }
    }
}

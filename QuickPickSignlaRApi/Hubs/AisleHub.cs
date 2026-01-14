using Microsoft.AspNetCore.SignalR;
using QuickPickSignlaRApi.Models;

namespace QuickPickSignlaRApi.Hubs
{
    public class AisleHub : Hub
    {
        public async Task SendAisleAsync(Aisle aisle)
        {
            await Clients.All.SendAsync("AisleReceived", aisle);
        }
        public async Task SendUpdatedAisleAsync(Aisle aisle)
        {
            await Clients.All.SendAsync("AisleUpdated", aisle);
        }
        public async Task SendDeledAisleAsync(Aisle aisle)
        {
            await Clients.All.SendAsync("AisleDeleted", aisle);
        }
    }
}

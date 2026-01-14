using Microsoft.AspNetCore.SignalR;
using QuickPickSignlaRApi.Models;

namespace QuickPickSignlaRApi.Hubs
{
    public class TransactionHub : Hub
    {
        public async Task SendTransactionAsync(Sale sale)
        {
            await Clients.All.SendAsync("TransactionReceived", sale);
        }
        public async Task SendChangedTransactionAsync(Sale sale)
        {
            await Clients.All.SendAsync("TransactionChanged", sale);
        }
        public async Task SendDeletedTransactionAsync(Sale sale)
        {
            await Clients.All.SendAsync("TransactionDeleted", sale);
        }
    }
}

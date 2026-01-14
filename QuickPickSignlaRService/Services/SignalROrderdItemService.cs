using Microsoft.AspNetCore.SignalR.Client;
using QuickPickSignlaRService.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace QuickPickSignlaRService.Services
{
    public class SignalROrderdItemService
    {
        HubConnection? _hubConnection;
        public event Action<List<BoughtItem>>? ItemReceied;
        public event Action<List<BoughtItem>>? ItemUpdated;
        public event Action<List<BoughtItem>>? ItemDeleted;
        public async Task<string> ConnectSignlaR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/orderedItemHub")
                .WithAutomaticReconnect()
                .Build();
            if (_hubConnection != null && _hubConnection?.State == HubConnectionState.Connected)
                return "Connected";
            _hubConnection?.On<List<BoughtItem>> ("ItemReceived", items =>
            {
                ItemReceied?.Invoke(items);
            });
            _hubConnection?.On<List<BoughtItem>>("ItemChanged", items =>
            {
                ItemUpdated?.Invoke(items);
            });
            try
            {
                    await _hubConnection.StartAsync().ConfigureAwait(false);
                return "Connected";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        async Task EnsureConnectedAsync()
        {
            if (_hubConnection == null)
                await ConnectSignlaR();

            if (_hubConnection!.State != HubConnectionState.Connected)
                await _hubConnection.StartAsync().ConfigureAwait(false);
        }
        public async Task SendItem(List<BoughtItem> items)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendItemsAsync", items);
        }
        public async Task UpdateItem(List<BoughtItem> items)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendChangedItemsAsync", items);
        }
    }
}

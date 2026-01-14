using Microsoft.AspNetCore.SignalR.Client;
using QuickPickSignlaRService.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace QuickPickSignlaRService.Services
{
    public class SignalRItemService
    {
        HubConnection? _hubConnection;
        public event Action<Item>? ItemReceied;
        public event Action<Item>? ItemUpdated;
        public event Action<Item>? ItemDeleted;
        public async Task<string> ConnectSignlaR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/itemHub")
                .WithAutomaticReconnect()
                .Build();
            if (_hubConnection != null && _hubConnection?.State == HubConnectionState.Connected)
                return "Connected";
            _hubConnection?.On<Item>("ItemReceived", item =>
            {
                ItemReceied?.Invoke(item);
            });
            _hubConnection?.On<Item>("ItemUpDated", item =>
            {
                ItemUpdated?.Invoke(item);
            });
            _hubConnection?.On<Item>("ItemDeleted", item =>
            {
                ItemUpdated?.Invoke(item);
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

        public async Task SendItem(Item item)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendItemAsync", item).ConfigureAwait(false);
        }
        public async Task UpdateItem(Item item)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendUpdatedItemAsync", item);
        }
        public async Task DeleteItem(Item item)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendDeletedItemAsync", item);
        }
    }
}

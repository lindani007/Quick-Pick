using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using QuickPickSignlaRService.Models;
using System;
using System.Collections.Generic;
using System.Text;
namespace QuickPickSignlaRService.Services
{
    public class SignalROrderService
    {
        HubConnection? _hubConnection;
        public event Action<Order>? OrderReceied;
        public event Action<Order>? OrderUpdated;
        public event Action<Order>? OrderDeleted;
        public event Action<Order>? ReadyOrderReceive;
        public async Task<string> ConnectSignlaR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/orderHub")
                .WithAutomaticReconnect()
                .Build();
            if (_hubConnection != null && _hubConnection?.State == HubConnectionState.Connected)
                return "Connected";
            _hubConnection?.On<Order>("OrderReceived", order =>
            {
                OrderReceied?.Invoke(order);
            });
            _hubConnection?.On<Order>("ReadyOrderReceived", order =>
            {
                ReadyOrderReceive?.Invoke(order);
            });
            _hubConnection?.On<Order>("OrderChangeded", order =>
            {
                OrderUpdated?.Invoke(order);
            });
            _hubConnection?.On<Order>("OrderDeleted", order =>
            {
                OrderUpdated?.Invoke(order);
            });
            try
            {
                    await _hubConnection.StartAsync();
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
        public async Task SendOrder(Order order)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            order.OrderedBy = _hubConnection.ConnectionId;
            await _hubConnection.InvokeAsync("SendOrderAsync", order);
        }
        public async Task SendReadyOrder(Order order)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendReadyOrderAsync", order);
        }
        public async Task UpdateOrder(Order order)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendChangedOrderAsync", order);
        }
        public async Task DeleteOrder(Order order)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendDeletedOrderAsync", order);
        }
    }
}

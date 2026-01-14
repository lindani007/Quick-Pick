using Microsoft.AspNetCore.SignalR.Client;
using QuickPickSignlaRService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickSignlaRService.Services
{
    public class StatusService
    {
        HubConnection? _hubConnection;
        public event Action<Order>? MobileStatus;
        public event Action<string>? DesktopStatus;
        public async Task<string> ConnectSignlaR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/statusHub")
                .WithAutomaticReconnect()
                .Build();
            if (_hubConnection != null && _hubConnection?.State == HubConnectionState.Connected)
                return "Connected";
            _hubConnection?.On<Order>("MobileStatusReceived", order =>
            {
                MobileStatus?.Invoke(order);
            });
            _hubConnection?.On<string>("DesktopStatusReceived", status =>
            {
                DesktopStatus?.Invoke(status);
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
        public async Task SendMobileStatus(Order order)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendStatusAsync", order);
        }
        public async Task SendDesktopStatus(string status)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("DesktopStatusReceived", status);
        }
    }
}

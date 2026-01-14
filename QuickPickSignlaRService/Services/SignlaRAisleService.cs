using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using QuickPickSignlaRService.Models;
namespace QuickPickSignlaRService.Services
{
    public class SignlaRAisleService
    {
        HubConnection? _hubConnection;
        public event Action<Aisle>? AisleReceied;
        public event Action<Aisle>? AisleUpdated;
        public event Action<Aisle>? AisleDeleted;
        public async Task<string> ConnectSignlaR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/aisleHub")
                .WithAutomaticReconnect()
                .Build();
            if (_hubConnection != null && _hubConnection?.State == HubConnectionState.Connected)
                return "Connected";
            _hubConnection?.On<Aisle>("AisleReceived", aisle =>
            {
                AisleReceied?.Invoke(aisle);
            });
            _hubConnection?.On<Aisle>("AisleUpdated", aisle =>
            {
                AisleUpdated?.Invoke(aisle);
            });
            _hubConnection?.On<Aisle>("AisleDeleted", aisle =>
            {
                AisleDeleted?.Invoke(aisle);
            });
            try
            {
                   await  _hubConnection.StartAsync().ConfigureAwait(false);
                return "Connected";
            }
            catch(Exception ex) 
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
        public async Task SendAisle(Aisle aisle)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
              await _hubConnection.InvokeAsync("SendAisleAsync", aisle).ConfigureAwait(false);
        }
        public async Task UpdateAisle(Aisle aisle)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
                await _hubConnection.InvokeAsync("SendUpdatedAisleAsync", aisle);
        }
        public async Task DeleteAisle(Aisle aisle)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
                await _hubConnection.InvokeAsync("SendDeledAisleAsync", aisle);
        }
    }
}

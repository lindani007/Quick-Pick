using Microsoft.AspNetCore.SignalR.Client;
using QuickPickSignlaRService.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickPickSignlaRService.Services
{
    public class TransactionService
    {
        HubConnection? _hubConnection;
        public event Action<Sale>? TransactionReceied;
        public event Action<Sale>? TransactionUpdated;
        public event Action<Sale>? TransactionDeleted;
        public async Task<string> ConnectSignlaR()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7079/aisleHub")
                .WithAutomaticReconnect()
                .Build();
            if (_hubConnection != null && _hubConnection?.State == HubConnectionState.Connected)
                return "Connected";
            _hubConnection?.On<Sale>("TransactionReceived", tran =>
            {
                TransactionReceied?.Invoke(tran);
            });
            _hubConnection?.On<Sale>("TransactionChanged", t =>
            {
                TransactionUpdated?.Invoke(t);
            });
            _hubConnection?.On<Sale>("TransactionDeleted", t =>
            {
                TransactionDeleted?.Invoke(t);
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
        public async Task SendTransaction(Sale s)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendTransactionAsync", s);
        }
        public async Task UpdateAisle(Sale s)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendChangedTransactionAsync", s);
        }
        public async Task DeleteAisle(Sale s)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await EnsureConnectedAsync();
            await _hubConnection.InvokeAsync("SendDeletedTransactionAsync", s);
        }
    }
}

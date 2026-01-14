using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick.QuickPickEmployer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace QuickPick.QuickPickEmployer.ViewModel
{
    public partial class TransactionListViewModel : ObservableObject
    {
        private const string Tfile = "transactions.json";
        private const string Ofile = "orders.json";
        [ObservableProperty]
        bool isTransactionEmpty = false;
        [ObservableProperty]
        bool isOrderEmpty = false;
        [ObservableProperty]
        ObservableCollection<Transaction> transactions = new ObservableCollection<Transaction>();
        [ObservableProperty]
        ObservableCollection<Order> orders = new ObservableCollection<Order>();
        string fullPathT = Path.Combine(FileSystem.AppDataDirectory, Tfile);
        string fullPathO = Path.Combine(FileSystem.AppDataDirectory, Ofile);
        public TransactionListViewModel()
        {
            LoadTransactions();
        }
        private void LoadTransactions()
        {
            if (File.Exists(fullPathT))
            {
                string json = File.ReadAllText(fullPathT);
                var transactionList = JsonSerializer.Deserialize<List<Transaction>>(json);
                List<Transaction> todayTransactions = new List<Transaction>();
                todayTransactions = transactionList.Where(t => t.TransactionDate == DateTime.Today).ToList();
                if (transactionList.Count > 0)
                {
                    foreach (var transaction in todayTransactions)
                    {
                        foreach (var t in Transactions)
                        {
                            if (t.ItemId == transaction.ItemId)
                            {
                                continue;
                            }
                            else
                            {
                                Transactions.Add(transaction);
                            }
                        }
                    }
                }
                else
                {
                    IsTransactionEmpty = true;
                    IsOrderEmpty = false;
                }
            }
            else
            {
                IsTransactionEmpty = true;
            }
        }
        private void LoadOrders()
        {
            if (File.Exists(fullPathO))
            {
                string json = File.ReadAllText(fullPathO);
                var orderList = JsonSerializer.Deserialize<List<Order>>(json);
                if (orderList.Count > 0)
                {
                   Orders = new ObservableCollection<Order>(orderList);
                }
                else
                {
                    IsOrderEmpty = true;
                    IsTransactionEmpty = false;
                }
            }
            else
            {
                IsOrderEmpty = true;
            }
        }
        [ObservableProperty]
        bool viewByItemName = true;
        [ObservableProperty]
        bool viewByOrderId = false;

        [RelayCommand]
        void ShowByItemName()
        {
            LoadTransactions();
            ViewByItemName = true;
            ViewByOrderId = false;
        }
        [RelayCommand]
        void ShowByOrderId()
        {
            LoadOrders();
            ViewByOrderId = true;
            ViewByItemName = false;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick.QuickPickEmployer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace QuickPick.QuickPickEmployer.ViewModel
{
    public partial class TodayTransactionViewModel : ObservableObject
    {
        private const string Tfile = "transactions.json";
        private const string Ofile = "orders.json";
        [ObservableProperty]
        bool isTransactionEmpty = false;
        [ObservableProperty]
        bool isOrderEmpty = false;
        [ObservableProperty]
        string total;
        [ObservableProperty]
        ObservableCollection<Transaction> transactions = new ObservableCollection<Transaction>();
        [ObservableProperty]
        ObservableCollection<Order> orders = new ObservableCollection<Order>();
        string fullPathT = Path.Combine(FileSystem.AppDataDirectory, Tfile);
       
        string fullPathO = Path.Combine(FileSystem.AppDataDirectory, Ofile);
        public TodayTransactionViewModel()
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
                if (transactionList.Count > 0)
                {
                    todayTransactions = transactionList.Where(t => t.TransactionDate == DateTime.Today).ToList();
                    Transactions = new ObservableCollection<Transaction>( todayTransactions.Where(d => d.ItemId == d.ItemId).ToList());
                    Total = $"Total Todays's Transaction : {Transactions.Sum(s => s.TotalAmount).ToString("C",new CultureInfo("en-ZA"))}";
                    //foreach (var transaction in todayTransactions)
                    //{
                    //    foreach (var t in Transactions)
                    //    {
                    //        if (t.ItemId == transaction.ItemId)
                    //        {
                    //            continue;
                    //        }
                    //        else
                    //        {
                    //            Transactions.Add(transaction);
                    //        }
                    //    }
                    //}
                }
                else
                {
                    IsTransactionEmpty = true;
                    IsOrderEmpty = false;
                }
            }
            else
            {
                List<Transaction> list = new List<Transaction>();
                string json = JsonSerializer.Serialize(list);
                File.WriteAllText(fullPathT, json); 
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
                    Orders = new ObservableCollection<Order>(orderList.Where(o => o.OrderDate == DateTime.Today).ToList());
                }
                else
                {
                    IsOrderEmpty = true;
                    IsTransactionEmpty = false;
                }
            }
            else
            {
                List<Order> list = new List<Order>();
                string json = JsonSerializer.Serialize(list);
                File.WriteAllText(fullPathT, json);
                IsOrderEmpty = true;
            }
        }

        [ObservableProperty]
        bool viewByItemName = true;
        [ObservableProperty]
        bool viewByOrderId = false;
        [RelayCommand]
        private async Task ViewByIteName()
        {
            LoadTransactions();
            ViewByItemName = true;
            ViewByOrderId = false;
        }
        [RelayCommand]
        private async Task ViewByOrderID()
        {
            LoadOrders();
            ViewByItemName = false;
            ViewByOrderId = true;
        }
       
    }
}

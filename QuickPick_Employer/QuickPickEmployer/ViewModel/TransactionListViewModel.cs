using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick_Employer.QuickPickEmployer.Models;
using QuickPick_Employer.QuickPickEmployer.Views;
using QuickPickDBApiService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel
{
    public partial class TransactionListViewModel : ObservableObject
    {
        List<Item> ItemsToSearchOn = new List<Item>();
        List<Order> OrdersToSearchOn = new List<Order>();
        [ObservableProperty]
        bool isTransactionEmpty = false;
        [ObservableProperty]
        bool isOrderEmpty = false;
        [ObservableProperty]
        Item selectedItem;
        [ObservableProperty]
        ObservableCollection<Item> items = new ObservableCollection<Item>();
        [ObservableProperty]
        ObservableCollection<Order> orders = new ObservableCollection<Order>();
        [ObservableProperty]
        ObservableCollection<BoughtItem> boughtItemsColl = new ObservableCollection<BoughtItem>();
        [ObservableProperty]
        ObservableCollection<BoughtItem> orderedItems = new ObservableCollection<BoughtItem>();
        SlesService _saleService;
        ItemService _itemService;
        OrderService _orderervice;
        DbBoughtItemService _dbBoughtItemService;
        public TransactionListViewModel(SlesService slesService,  
            ItemService itemService, OrderService orderervice, DbBoughtItemService dbBoughtItemService,ItemService itemService1)
        {
            _itemService = itemService;
            this._saleService = slesService;
            LoadTransactions();
            _itemService = itemService;
            _orderervice = orderervice;
            _dbBoughtItemService = dbBoughtItemService;
        }
        private async void LoadTransactions()
        {
            BoughtItemsColl = new ObservableCollection<BoughtItem>( await ReturnBoughtItemsByNames());
            List<Item> list = new List<Item>();
                if (BoughtItemsColl.Count > 0) 
                {
                bool itemExist = false;
                        foreach (var item in BoughtItemsColl)
                        {
                            Item itm = new Item();
                            Item i = new Item();
                            i.ItemId = item.ItemId;
                            i.ItemName = item.ItemName;
                            i.ItemQuantity = item.Quantity;
                            i.ImageSource = item.ImageSourceUrl;
                            if(list.Count > 0)
                            {
                                 itemExist  = list.Any(x => x.ItemId == item.ItemId);
                            }
                            if(!itemExist)
                            {
                               list.Add(i);
                               continue;
                            }
                            i = list.Where(x => x.ItemId == item.ItemId).FirstOrDefault() ?? new Item();
                            i.ItemQuantity += item.Quantity;
                        }
                }
                else
                {
                    IsTransactionEmpty = true;
                    IsOrderEmpty = false;
                }
            Items = new ObservableCollection<Item>(list);
            ItemsToSearchOn = list;
        }
        private async void LoadOrders()
        {
            var l = await _orderervice.GetOrdersAsync();
            List<Order> List = l.Select(t => new Order
            {
                OrderId = t.Id,
                OrderNumber = t.Order_Number,
                OrderDate = t.Occured_On,
                OrderedItemsQty = t.Quantity,
                Code = t.Code
            }).OrderByDescending(x => x.OrderDate).ToList();
                if (List.Count > 0)
                {
                   Orders = new ObservableCollection<Order>(List);
                   OrdersToSearchOn = List;
                }
                else
                {
                    IsOrderEmpty = true;
                    IsTransactionEmpty = false;
                }
        }
        [ObservableProperty]
        bool viewByItemName = true;
        [ObservableProperty]
        bool viewByOrderId = false;
        [ObservableProperty]
        Color nameButtonColor = Colors.LightGreen;
        [ObservableProperty]
        Color orderButtonColor = Colors.LimeGreen;
        [ObservableProperty]
        string? searchItem;
        [ObservableProperty]
        string? searchOrder;
        [ObservableProperty]
        Order selectedOrder;
        [ObservableProperty]
        string? quantityToRefund;
        [ObservableProperty]
        string? itemName;
        [ObservableProperty]
        ImageSource? imageSourceUrl;
        [ObservableProperty]
        double price;
        [ObservableProperty]
        int quantity;
        BoughtItem ItemToRefund;
        [ObservableProperty]
        string emptyQuantity;
        [ObservableProperty]
        string? code;
        double _Price;
        bool IsFromHere = false;
        [RelayCommand]
        async Task ShowByItemName()
        {
            LoadTransactions();
            ViewByItemName = true;
            ViewByOrderId = false;
            NameButtonColor = Colors.LightGreen;
            OrderButtonColor = Colors.LimeGreen;
        }
        [RelayCommand]
        void ShowByOrderId()
        {
            LoadOrders();
            ViewByOrderId = true;
            ViewByItemName = false;
            OrderButtonColor = Colors.LightGreen;
            NameButtonColor = Colors.LimeGreen;
        }
        [RelayCommand]
        private async Task ItemToViewItTransactions()
        {
            if (SelectedItem != null)
            {
                var l = await ReturnBoughtItemsByNames();
                l = l.OrderByDescending(d => d.TransactionDtae).ToList();
                List<BoughtItem> list = l.Where(i => i.ItemId == SelectedItem.ItemId).ToList();
                await App.Current.MainPage.Navigation.PushAsync(new DetailedTRansaction(list));
                SelectedItem = null;
            }
        }
        [RelayCommand]
        public async Task GetSelectedOrder()
        {
            if(SelectedOrder != null)
            {
                OrderedItems = new ObservableCollection<BoughtItem>(await ReturnBoughtItemsForOrder(SelectedOrder));
                await App.Current.MainPage.Navigation.PushAsync(new Ordered_Items(this));
                SelectedOrder = null;
            }
        }
        [RelayCommand]
        public async Task ViewItemTransaction(BoughtItem item)
        {
                var l = await ReturnBoughtItemsByNames();
                l = l.OrderByDescending(d => d.TransactionDtae).ToList();
                List<BoughtItem> list = l.Where(i => i.ItemId == item.ItemId).ToList();
                await App.Current.MainPage.Navigation.PushAsync(new DetailedTRansaction(list));
        }
        [RelayCommand]
        private async Task RefundItems(BoughtItem item)
        {
            _Price = item.Price;
            ItemToRefund = item;
            ItemName = item.ItemName;
            Quantity = item.Quantity;
            Price = item.Price * item.Quantity;
            ImageSourceUrl = item.ImageSourceUrl;
          await  App.Current.MainPage.Navigation.PushAsync(new Refund_Item(this));
        }
        [RelayCommand]
        private async Task Refund()
        {
            if (string.IsNullOrEmpty(QuantityToRefund))
            {
               await App.Current.MainPage.DisplayAlertAsync("Refund", "Quantity to refund is empty", "Retry");
                return;
            }
            if(string.IsNullOrEmpty(Code))
            {
                await App.Current.MainPage.DisplayAlertAsync("Refund", "Order code is empty", "Retry");
                return;
            }
            else if(Code.Trim() != ItemToRefund.Code)
            {
                await App.Current.MainPage.DisplayAlertAsync("Refund", "Code does not match with the order code", "Retry");
                return;
            }
                ItemToRefund.Quantity = int.Parse(QuantityToRefund);
            await  App.Current.MainPage.Navigation.PopAsync();
        }
        partial void OnSearchItemChanged(string? value)
        {
            var list = new List<Item>();
            foreach (var item in ItemsToSearchOn)
            {
                if (item.ItemName.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                  || item.ItemName.Contains(value, StringComparison.OrdinalIgnoreCase)
                  || item.ItemName.EndsWith(value, StringComparison.OrdinalIgnoreCase))
                {
                    if (!string.IsNullOrEmpty(SearchItem))
                    {
                        //EmptyItems = string.Empty;
                    }
                    list.Add(item);
                }
            }
            if (!(list.Count > 0))
            {
               // EmptyItems = "Item with that name was not found";
            }
            if (string.IsNullOrEmpty(value))
            {
               // EmptyItems = string.Empty;
                Items = new ObservableCollection<Item>(ItemsToSearchOn);
            }
            Items = new ObservableCollection<Item>(list);
        }
        partial void OnQuantityToRefundChanged(string? value)
        {
            if(string.IsNullOrEmpty(value))
            {
                Price = Quantity * _Price;
                EmptyQuantity = "Quantity is required";
            }
            else if(!int.TryParse(value, out int _) || int.Parse(value) <= 0)
            {
                Price = Quantity * _Price;
                EmptyQuantity = "Quantity must be a valid number";
            }
            else
            {
                if(int.Parse(value) > Quantity)
                {
                    Price = Quantity * _Price;
                    EmptyQuantity = "The quantity you want to refund i bigger than the quantity that was ordered";
                    return;
                }
                EmptyQuantity = string.Empty;
                if (int.Parse(value) <= Quantity)
                {
                    Price = (Quantity - int.Parse(value)) * _Price;
                }
            }
        }
        partial void OnSearchOrderChanged(string? value)
        {
            var list = new List<Order>();
            foreach (var item in OrdersToSearchOn)
            {
                if (!string.IsNullOrEmpty(item.OrderNumber))
                {
                    if (item.OrderNumber.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                         || item.OrderNumber.Contains(value, StringComparison.OrdinalIgnoreCase)
                         || item.OrderNumber.EndsWith(value, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.IsNullOrEmpty(SearchItem))
                        {
                            //EmptyItems = string.Empty;
                        }
                        list.Add(item);
                    }
                }
            }
            if (!(list.Count > 0))
            {
                // EmptyItems = "Item with that name was not found";
            }
            if (string.IsNullOrEmpty(value))
            {
                // EmptyItems = string.Empty;
                Orders = new ObservableCollection<Order>(OrdersToSearchOn);
            }
            Orders = new ObservableCollection<Order>(list);
        }
        private async Task<List<Item>> ReturnItemsList()
        {
            var l = await _itemService.GetItemsAsync();
            List<Item> Items = l.Select(propa => new Item
            {
                ItemId = propa.ID,
                ItemName = propa.Item_Name,
                ItemDescription = propa.Description,
                ItemPrice = propa.Price,
                ItemQuantity = propa.Quantity,
                LeftQuantity = propa.LeftQuantity,
                AisleId = propa.Aisle_Id,
                ImageSource = propa.ImageUrl,
            }).ToList();
            return Items;
        }
        private async Task<List<BoughtItem>> GetItems()
        {
            List<Item> Items = await ReturnItemsList();
            var list = await _dbBoughtItemService.GetItemsAsync();
            List<BoughtItem> itemsList = list.Select(x => new BoughtItem
            {
                TransactionId = x.TransactionId,
                OrderId = x.Order_Id,
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                TotalAmount = x.TotalAmount,
                Price = x.Price,
                Quantity = x.Quantity,
                Packed_By = x.Packed_By,
                TransactionDtae = x.TransactionDtae,
                ImageSourceUrl = x.ImageSourceUrl
            }).ToList();
            return itemsList;
        }
        private async Task<List<BoughtItem>> ReturnBoughtItemsByNames()
        {
            List<BoughtItem> itemsBought = await GetItems();
            return itemsBought;
        }
        private async Task<List<BoughtItem>> ReturnBoughtItemsForOrder(Order order)
        {
            List<BoughtItem> items = await GetItems();
            items = items.Where(x => x.OrderId == order.OrderId).ToList();
            List<BoughtItem> uniqueList = new List<BoughtItem>();
            bool itemExist = false;
            foreach (var item in items)
            {
                if(uniqueList.Count > 0)
                {
                    itemExist = uniqueList.Any(x => x.ItemId == item.ItemId);
                }
                if(!itemExist)
                {
                    uniqueList.Add(item);
                    continue;
                }
                BoughtItem i = uniqueList.Where(x => x.ItemId == item.ItemId).FirstOrDefault() ?? new BoughtItem();
                uniqueList.Remove(i);
                i.Quantity += item.Quantity;
                uniqueList.Add(i);
            }
            return uniqueList;
        }
    }
}
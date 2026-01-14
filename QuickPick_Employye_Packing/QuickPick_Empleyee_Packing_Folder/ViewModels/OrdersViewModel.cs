using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Models;
using QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.Views;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace QuickPick_Employye_Packing.QuickPick_Empleyee_Packing_Folder.ViewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private int totalItems;

        [ObservableProperty]
        bool packed;

        int id = 0;

        [ObservableProperty]
        string employeeId;

        [ObservableProperty]
        string employeeName;

        [ObservableProperty]
        string password;

        //Collections
        [ObservableProperty]
        ObservableCollection<Order> orders;
        [ObservableProperty]
        private ObservableCollection<OrderedItems> ordereditems;
        [ObservableProperty]
        private ObservableCollection<OrderedItems> itemsToBePacked;

        //Selected Items
        [ObservableProperty]
        private Order selectedOrder;
        [ObservableProperty]
        private OrderedItems selectedOrderItem;


        // Services and Models
        QuickPickSignlaRService.Models.Order _order;
        QuickPickDBApiService.Models.ApiModels.Order _dbOrder;
        SignalROrderService _signalROrderService;
       SignalROrderdItemService _signalROrderdItemService;

        // API Services
        OrderService _orderService;
        DbBoughtItemService _dbBoughtItemService;
        LoginService _loginService;

        public OrdersViewModel(SignalROrderdItemService signalROrderdItemService,SignalROrderService signalROrderService,OrderService orderService,DbBoughtItemService dbBoughtItemService,LoginService loginService)
        {
            _signalROrderdItemService = signalROrderdItemService;
            _signalROrderService = signalROrderService;
            _dbBoughtItemService = dbBoughtItemService;
            _orderService = orderService;
            _loginService = loginService;
            StartConetion();
            LoadItems();
            LoadOrders();
        }
        private async void StartConetion()
        {
            await _signalROrderService.ConnectSignlaR();
            await _signalROrderdItemService.ConnectSignlaR();
            _signalROrderService.OrderReceied -= OrderReceived;
            _signalROrderService.OrderReceied += OrderReceived;
            _signalROrderdItemService.ItemReceied -= ItemsReceived;
            _signalROrderdItemService.ItemReceied += ItemsReceived;    
        }
        [RelayCommand]

        private async Task GetSelectedOrder()
        {
            if (SelectedOrder != null)
            {
                id = selectedOrder.OrderId;
                _order = new QuickPickSignlaRService.Models.Order()
                {
                    OrderId = SelectedOrder.OrderId,
                    OrderNumber = SelectedOrder.OrderNumber,
                    Code = SelectedOrder.Code,
                    OrderedItemsQty = SelectedOrder.OrderedItemsQty,
                    OrderDate = SelectedOrder.OrderDate,
                    Status = SelectedOrder.Status,
                    TotalAmount = SelectedOrder.TotalAmount,
                    OrderedBy = SelectedOrder.OrderedBy,
                };
                _dbOrder = new QuickPickDBApiService.Models.ApiModels.Order
                {
                    Id = SelectedOrder.OrderId,
                    Order_Number = SelectedOrder.OrderNumber,
                    Code = SelectedOrder.Code,
                    Occured_On = SelectedOrder.OrderDate,
                    Quantity = SelectedOrder.OrderedItemsQty,
                    Price = SelectedOrder.TotalAmount,
                    Status = SelectedOrder.Status,
                };
                List<OrderedItems> items = Ordereditems.Where(x => x.OrderId == SelectedOrder.OrderId).ToList();
                ItemsToBePacked = new ObservableCollection<OrderedItems>(items.Select(x => new OrderedItems
                {
                    OrderId = x.OrderId,
                    ItemId = x.ItemId,
                    ItemName = x.ItemName,
                    TotalAmount = x.TotalAmount,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    TransactionDtae = x.TransactionDtae,
                    ImageSourceUrl = x.ImageSourceUrl
                }).ToList());
                TotalItems = ItemsToBePacked.Select(i => i.Quantity).Sum();
                await App.Current.MainPage.Navigation.PushAsync(new PackingPage(this));
                SelectedOrder = null;
            }
            await Task.CompletedTask;
        }
        int packedItems = 0;
        [RelayCommand]
        private async Task GetSelectedItem()
        {
            if (SelectedOrderItem == null)
                return;
         string answer = await  App.Current.MainPage.DisplayActionSheetAsync($"Did you packed {SelectedOrderItem.Quantity} {SelectedOrderItem.ItemName}(s)","Yes", "No");
            if(answer == "Yes")
            {
                packedItems += SelectedOrderItem.Quantity;
                ItemsToBePacked.Remove(SelectedOrderItem);
                ItemsToBePacked = new ObservableCollection<OrderedItems>(ItemsToBePacked);
                
                SelectedOrderItem = null;
                return;
            }
            await App.Current.MainPage.DisplayAlertAsync(SelectedOrderItem.ItemName, "Please pack the item well","Ok");
            SelectedOrderItem = new OrderedItems();
        }
        [RelayCommand]
        private async Task DonePacking()
        {
            if(packedItems == TotalItems)
            {
                _order.Status = "Packed";
                _dbOrder.Status = "Packed";
                await _signalROrderService.SendReadyOrder(_order);
                await _orderService.UpdateOrderAsync(_dbOrder);
                packedItems = 0;
                ItemsToBePacked.Clear();
                SelectedOrder = new Order();
                await App.Current.MainPage.DisplayAlertAsync("Success", "Packing Completed!", "OK");
                await App.Current.MainPage.Navigation.PopAsync();
                await App.Current.MainPage.Navigation.PushAsync(new OrderReceivingPage(this));
                var o = Orders.SingleOrDefault(x => x.OrderId == id);
                Orders.Remove(o);
                Orders = new ObservableCollection<Order>(Orders);
                return;
            }
          await  App.Current.MainPage.DisplayAlertAsync("Order", "You have not finished to pack all the items or quntity ordered", "Re Pack");
        }
        private async Task<string> VerifyEmployee()
        {
            var list = await _loginService.GetLoginAsync();
            var emp = list.SingleOrDefault(x => x.EmployeeID == EmployeeId);
            if (emp != null)
            {
                if(emp.Password == Password && emp.EmployeeID == EmployeeId)
                {
                    return "Success";
                }
                else
                {
                    return "Invalid password please verify your password.";
                }
            }
            return "Invalid Employee ID please verify your ID.";
        }
        [RelayCommand]
        private async Task LoginEmployee()
        {
            if(string.IsNullOrEmpty(EmployeeId) || string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlertAsync("Login Failed", "Please fill all the fields", "Ok");
                return;
            }
            string result = await VerifyEmployee();
            if(result == "Success")
            {
                await App.Current.MainPage.Navigation.PopAsync();
                await App.Current.MainPage.Navigation.PopAsync();
                await App.Current.MainPage.Navigation.PushAsync(new OrderReceivingPage(this));
                EmployeeId = string.Empty;
                Password = string.Empty;
                return;
            }
            await App.Current.MainPage.DisplayAlertAsync("Login Failed", result, "Ok");
        }
        private async void LoadOrders()
        {
            var l = await _orderService.GetOrdersAsync();
            l = l.Where(x => x.Status == "Received").ToList();
            List<Order> List = l.Select(t => new Order
            {
                OrderId = t.Id,
                OrderNumber = t.Order_Number,
                OrderDate = t.Occured_On,
                OrderedItemsQty = t.Quantity,
                Code = t.Code,
                Status = t.Status,
            }).ToList();
            Orders = new ObservableCollection<Order>(List);
        }
        private async void LoadItems()
        {
            var list = await _dbBoughtItemService.GetItemsAsync();
            List<OrderedItems> itemsList = list.Select(x => new OrderedItems
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
            Ordereditems = new ObservableCollection<OrderedItems>(itemsList);
        }
        public async void OrderReceived(QuickPickSignlaRService.Models.Order order)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadItems();
                Order o = new Order
                {
                    OrderId = order.OrderId,
                    OrderedItemsQty = order.OrderedItemsQty,
                    Code = order.Code,
                    OrderNumber = order.OrderNumber,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderedBy = order.OrderedBy,
                    Status = order.Status
                };
                Orders.Add(o);
                Orders = new ObservableCollection<Order>(Orders);
              
            });
        }
        public async void ItemsReceived(List<QuickPickSignlaRService.Models.BoughtItem> boughtItems)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                LoadItems();
            });
        }
    }
}

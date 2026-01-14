using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Order_Distribution.OrderDistribution.Models;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Order_Distribution.OrderDistribution.ViwewModels
{
    public partial class OrdersViewModel : ObservableObject
    {
        [ObservableProperty]
        ObservableCollection<Order> orders = new ObservableCollection<Order>();
        [ObservableProperty]
        private Order? selectedOrder;
        [ObservableProperty]
        private Order? selectedOrder2;
        [ObservableProperty]
        private ObservableCollection<Order> ordersNotCollected = new ObservableCollection<Order>();
        [ObservableProperty]
        string code;
        [ObservableProperty]
        string orderNumber;
        QuickPickDBApiService.Models.ApiModels.Order _order = new QuickPickDBApiService.Models.ApiModels.Order();
        OrderService    _orderService;
        SignalROrderService _SignalROrderService;
        Order orderToVerify;
        bool IsFromNewOrders = false;
        public OrdersViewModel(OrderService orderService, SignalROrderService signalROrderService)
        {
            _orderService = orderService;
            _SignalROrderService = signalROrderService;
            StartConnection();
            LoadOrders();
        }
        private async void StartConnection()
        {
          await  _SignalROrderService.ConnectSignlaR();
            _SignalROrderService.ReadyOrderReceive -= OrderReceived;
            _SignalROrderService.ReadyOrderReceive += OrderReceived;
        }
        [RelayCommand]
        private async Task GetSelectedOrder()
        {
            if (SelectedOrder != null)
            {
                _order = new()
                {
                    Id = SelectedOrder.OrderId,
                    Order_Number = SelectedOrder.OrderNumber,
                    Occured_On = SelectedOrder.OrderDate,
                    Quantity = SelectedOrder.OrderedItemsQty,
                    Code = SelectedOrder.Code,
                    Status = "Collected",
                };
                orderToVerify = SelectedOrder;
                OrderNumber = $"Code For Order {SelectedOrder.OrderNumber}";
                string answer =  await App.Current.MainPage.DisplayActionSheetAsync($"Does a customer come to collect order {SelectedOrder.OrderNumber}?","Yes", "No");
                if(answer == "Yes")
                {
                    IsFromNewOrders = true;
                    SelectedOrder = null;
                    await App.Current.MainPage.Navigation.PushAsync(new Views.OrderVerification(this));
                }
                else if(answer == "No")
                {
                    await App.Current.MainPage.DisplayAlertAsync("Info", "Please wait for the customer to arrive. The order has bee added to orders that are waiting for collection.", "OK");
                    OrdersNotCollected.Add(SelectedOrder);
                    Orders.Remove(SelectedOrder);
                    SelectedOrder = null;
                }
                
            }
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task GetSelectedOrderInUncollectedOrders()
        {
            if (SelectedOrder2 != null)
            {
                orderToVerify = SelectedOrder2;
                string answer = await App.Current.MainPage.DisplayActionSheetAsync($"Does a customer come to collect this order {SelectedOrder2.OrderNumber}?", "Cancel", null, "Yes", "No");
                if (answer == "Yes")
                {
                    SelectedOrder2 = null;
                    await App.Current.MainPage.Navigation.PushAsync(new Views.OrderVerification(this));
                }
                else if (answer == "No")
                {
                    await App.Current.MainPage.DisplayAlertAsync("Info", "Please wait for the customer to arrive. The order os added to orders that are waiting for collection.", "OK");
                }

            }
            SelectedOrder2 = null;
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task VerifyOder()
        {
            if(!string.IsNullOrEmpty(Code))
            {
                if(orderToVerify != null)
                {
                    if(IsFromNewOrders)
                    {
                        IsFromNewOrders = false;
                        if (Code == orderToVerify.Code)
                        {
                            await _orderService.UpdateOrderAsync(_order);
                            Order order = Orders.FirstOrDefault(t => t.OrderId == orderToVerify.OrderId) ?? new Order();
                            Orders.Remove(order);
                            await App.Current.MainPage.DisplayAlertAsync("Success", "The code is correct. You may proceed to hand over the order to the customer.", "OK");
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlertAsync("Error", "The code is incorrect. Please double-check the code and try again.", "OK");
                        }
                    }
                    else
                    {
                        if (Code == orderToVerify.Code)
                        {
                            await _orderService.UpdateOrderAsync(_order);
                            Order order = OrdersNotCollected.FirstOrDefault(t => t.OrderId == orderToVerify.OrderId) ?? new Order();
                            OrdersNotCollected.Remove(order);
                            await App.Current.MainPage.DisplayAlertAsync("Success", "The code is correct. You may proceed to hand over the order to the customer.", "OK");
                            await App.Current.MainPage.Navigation.PopAsync();
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlertAsync("Error", "The code is incorrect. Please double-check the code and try again.", "OK");
                        }
                    }
                }
                else
                {
                    await App.Current.MainPage.DisplayAlertAsync("Error", "No order selected for verification.", "OK");
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlertAsync("Error", "Please enter a code to verify.", "OK");
            }
        }
        private async void LoadOrders()
        {
            var l = await _orderService.GetOrdersAsync();
            l = l.Where(t => t.Status == "Packed").ToList();
            List<Order> List = l.Select(t => new Order
            {
                OrderId = t.Id,
                OrderNumber = t.Order_Number,
                OrderDate = t.Occured_On,
                OrderedItemsQty = t.Quantity,
                Code = t.Code,
                Status = t.Status,
            }).ToList();
            OrdersNotCollected = new ObservableCollection<Order>(List);
        }
        private async void OrderReceived(QuickPickSignlaRService.Models.Order order)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Order o = new Order
                {
                    OrderId = order.OrderId,
                    Code = order.Code,
                    OrderDate = order.OrderDate,
                    OrderNumber = order.OrderNumber,
                    OrderedItemsQty = order.OrderedItemsQty,
                    OrderedBy = order.OrderedBy,
                    Status = "Ready",
                };
                Orders.Add(o);
                Orders = new ObservableCollection<Order>(Orders);

            });
        }
    }
}

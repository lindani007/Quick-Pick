
using QuickPick_Customer.QuieckPickCustomer.Models;
using QuickPickDBApiService.Services;
using System;
using System.Collections.Generic;
using System.Text;
using SecurityApi;
using System.Globalization;

namespace QuickPick_Customer.QuieckPickCustomer.ViewModels
{
    public class OrderGenarater
    {
        OrderService _orderService;
        ItemService _itemService;
        public OrderGenarater(OrderService orderService,ItemService itemService)
        {
            this._orderService = orderService;
            _itemService = itemService;
        }
        public async Task<string> CreateOrderNumber()
        {
            List<Order> list = await ReturnOrders();
            int ordersPlacedTodayQty = list.Where(x => x.OrderDate.Date == DateTime.Today).Count();
            ordersPlacedTodayQty++;
            string orderNumber;
            if (ordersPlacedTodayQty < 10)
            {
             return orderNumber = $"#00{ordersPlacedTodayQty}";
            }
            if (ordersPlacedTodayQty < 100)
            {
                return orderNumber = $"#0{ordersPlacedTodayQty}";
            }
           return orderNumber = $"#{ordersPlacedTodayQty}";
        }
        public async Task<Order> CreateOrder(List<Item> items)
        {
            string code = SecurityApi.CodeGenarator.CreateCode();
            int quantity = items.Select(x => x.RequestedQuantity).Sum();
            string orderNumber = await CreateOrderNumber();
            double total = items.Select(x => x.ItemPrice * x.RequestedQuantity).Sum();
            Order order = new()
            {
                OrderDate = DateTime.Now,
                OrderNumber = orderNumber,
                OrderedItemsQty = quantity,
                Code = code,
                TotalAmount = total,
                Status = "Received",
            };
           await UpdateQuantityLeft(items);
            return order;
        }
        public async Task<QuickPickSignlaRService.Models.Order> CreateSignalROrder(Order order)
        {
            QuickPickSignlaRService.Models.Order o = new()
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                OrderedItemsQty = order.OrderedItemsQty,
                Code = order.Code,
                TotalAmount = order.TotalAmount,
                Status = "Received",
            };
            return o;
        }
        public async Task<QuickPickDBApiService.Models.ApiModels.Order> CreatedBROrder(List<Item> items)
        {
            string code = SecurityApi.CodeGenarator.CreateCode();
            int quantity = items.Select(x => x.RequestedQuantity).Sum();
            string orderNumber = await CreateOrderNumber();
            double total = items.Select(x => x.ItemPrice * x.RequestedQuantity).Sum();
            QuickPickDBApiService.Models.ApiModels.Order order = new()
            {
                Occured_On = DateTime.Now,
                Order_Number = orderNumber,
                Quantity = quantity,
                Code = code,
                Price = total,
                Status = "Received",
            };
            return order;
        }
        public async Task<List<QuickPickSignlaRService.Models.BoughtItem>> SignalRBoughtItems(List<Item> items, Order order)
        {
            List<QuickPickSignlaRService.Models.BoughtItem> orederedItems = items.Select(x => new QuickPickSignlaRService.Models.BoughtItem
            {
                OrderedId = order.OrderId,
                TransactionDtae = order.OrderDate,
                TotalAmount = x.ItemPrice * x.RequestedQuantity,
                Quantity = x.RequestedQuantity,
                Price = x.ItemPrice,
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                ImageSourceUrl = RetutnUri(x.ImageSource)
            }).ToList();
            return orederedItems;
        }
        public async Task<List<QuickPickDBApiService.Models.ApiModels.BoughtItem>> DbBoughtItems(List<Item> items, Order order)
        {
            List<QuickPickDBApiService.Models.ApiModels.BoughtItem> orederedItems = items.Select(x => new QuickPickDBApiService.Models.ApiModels.BoughtItem
            {
                Order_Id = order.OrderId,
                TransactionDtae = order.OrderDate,
                TotalAmount = x.ItemPrice * x.RequestedQuantity,
                Quantity = x.RequestedQuantity,
                Price = x.ItemPrice,
                ItemId = x.ItemId,
                ItemName = x.ItemName,
                ImageSourceUrl = RetutnUri(x.ImageSource)
            }).ToList();
            return orederedItems;
        }
        public async Task<string> CreateInvoice(List<Item> items,double total, double subtotal, double vat,Order order) 
        {
            string item = string.Empty;
            foreach (var i in items)
            {
                item += $"\n{i.ItemName}.....   {i.ItemPrice.ToString("C", new CultureInfo("en-ZA"))} x{i.RequestedQuantity}";
            }
            string invoice = $"\n\n\t\t*****Quick Pick*****\n\t\tOrder Number {order.OrderNumber}\n\t\tOrder Code {order.Code}\n\n{item}\n\nSubTotal : .....   {subtotal.ToString("C", new CultureInfo("en-ZA"))}\nVAT : ..............   {vat.ToString("C", new CultureInfo("en-ZA"))}\nTotal : .............   {total.ToString("C", new CultureInfo("en-ZA"))}\n";
            invoice += "\n\n        Thank you for using Quick Pick we hope you enjoyed our service\n\t\t\"Skip The Queue Grabb Your Goods\"";
            return invoice;
        }
        public async Task UpdateQuantityLeft(List<Item> Items)
        {
            foreach (var item in Items)
            {
                item.LeftQuantity =  item.LeftQuantity - item.RequestedQuantity;
                QuickPickDBApiService.Models.ApiModels.Item i = new()
                {
                    ID = item.ItemId,
                    Aisle_Id = item.AisleId,
                    Quantity = item.ItemQuantity,
                    LeftQuantity = item.LeftQuantity,
                    Price= item.ItemPrice,
                    Description = item.ItemDescription,
                    Item_Name = item.ItemName,
                };
                if(item.ImageSource is UriImageSource uri && uri.Uri != null)
                {
                    i.ImageUrl = uri.Uri.ToString();
                }
                await  _itemService.UpdateItemAsync(i);
            }
        }
        private async Task<List<Order>> ReturnOrders()
        {
            var l = await _orderService.GetOrdersAsync();
            List<Order> List = l.Select(t => new Order
            {
                OrderId = t.Id,
                OrderNumber = t.Order_Number,
                OrderDate = t.Occured_On,
                OrderedItemsQty = t.Quantity,
                Code = t.Code
            }).ToList();
            return List;

        }
        private string RetutnUri(ImageSource imageSource)
        {
            if(imageSource is UriImageSource uri && uri.Uri != null)
            {
                return uri.Uri.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

    }
}

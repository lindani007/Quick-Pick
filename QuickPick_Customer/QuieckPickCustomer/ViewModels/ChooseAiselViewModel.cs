using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick_Customer.QuieckPickCustomer.Models;
using QuickPick_Customer.QuieckPickCustomer.Views;
using QuickPickDBApiService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.Json;
using QuickPickSignlaRService.Services;
using System.Threading.Tasks;
namespace QuickPick_Customer.QuieckPickCustomer.ViewModels
{
    public partial class ChooseAiselViewModel : ObservableObject
    {
        List<Aisle> AislesList = new List<Aisle>();
        List<Item> ItemsList = new List<Item>();
        [ObservableProperty]
        List<Item> orderedItems = new List<Item>();
        string choosedAisle;
        int choosedAisleId;
        [ObservableProperty]
        private string? emptyAisle;
        [ObservableProperty]
        private string searchAisle;
        [ObservableProperty]
        private Aisle selectedAisle;
        [ObservableProperty]
        Item selectedItem;
        [ObservableProperty]
        string? searchItem;
        [ObservableProperty]
        string? itemQuantity = "1";
        [ObservableProperty]
        private double totalOrdered = 0;
        [ObservableProperty]
        ObservableCollection<Aisle> aisleCollection = new ObservableCollection<Aisle>();
        [ObservableProperty]
       ObservableCollection<Item> itemCollection = new ObservableCollection<Item>();
        double p;
        [ObservableProperty]
        int quantity = 0;
        Item ItemToBeBought;
        ItemService _itemService;
        AisleService _aislleService;
        SlesService _saleService;
        OrderService _orderService;
        SignlaRAisleService _signalRAsileService;
        SignalRItemService _signalRItemService;
        SignalROrderService _SignalROrderService;
        SignalROrderdItemService _signalROrderdItemService;
        DbBoughtItemService _dbBoughtItemService;
        public ChooseAiselViewModel(ItemService itemService,AisleService aislleService, 
            SlesService saleService, OrderService orderService, SignalRItemService signalRItemService,
             SignlaRAisleService signlaRAisleService,SignalROrderService signalROrderService
            ,SignalROrderdItemService signalROrderdItemService, DbBoughtItemService dbBoughtItemService)
        {
            this._itemService = itemService;
            _aislleService = aislleService;
            _saleService = saleService;
            _orderService = orderService;
            _signalRAsileService = signlaRAisleService;
            _signalRItemService = signalRItemService;
            _SignalROrderService = signalROrderService;
            _signalROrderdItemService = signalROrderdItemService;
            _dbBoughtItemService = dbBoughtItemService;
            StartConnection();
            LoadAisles();
        }
        public async void StartConnection()
        {
            await _signalRAsileService.ConnectSignlaR();
            await _signalRItemService.ConnectSignlaR();
            await _SignalROrderService.ConnectSignlaR();
            await _signalROrderdItemService.ConnectSignlaR();
            _signalRAsileService.AisleReceied -= RealTimeAisleUpdate;
            _signalRAsileService.AisleReceied += RealTimeAisleUpdate;
            _signalRAsileService.AisleDeleted -= DeleteAisle;
            _signalRAsileService.AisleDeleted += DeleteAisle;
            _signalRAsileService.AisleUpdated -= UpdateAisle;
            _signalRAsileService.AisleUpdated += UpdateAisle;

            _signalRItemService.ItemDeleted -= DeleteItemWhenDeleted;
            _signalRItemService.ItemDeleted += DeleteItemWhenDeleted;
            _signalRItemService.ItemUpdated -= ChangeItemWhenChanged;
            _signalRItemService.ItemUpdated += ChangeItemWhenChanged;
            _signalRItemService.ItemReceied -= AddRealTimeItem;
            _signalRItemService.ItemReceied += AddRealTimeItem;
        }
        partial void OnSearchAisleChanged(string value)
        {
            var list = new List<Aisle>();
            foreach (var item in AislesList)
            {
                if (item.AisleName.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.AisleName.Contains(value, StringComparison.OrdinalIgnoreCase)
                    || item.AisleName.EndsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.AisleDescription.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.AisleDescription.Contains(value, StringComparison.OrdinalIgnoreCase)
                    || item.AisleDescription.EndsWith(value, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(item);
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                AisleCollection = new ObservableCollection<Aisle>(AislesList);
            }
            AisleCollection = new ObservableCollection<Aisle>(list);
        }
        partial void OnSearchItemChanged(string? value)
        {
            var list = new List<Item>();
            foreach (var item in ItemsList)
            {
                if (string.IsNullOrEmpty(item.ItemName) || string.IsNullOrEmpty(item.ItemDescription))
                    continue;
                if (item.ItemName.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.ItemName.Contains(value, StringComparison.OrdinalIgnoreCase)
                    || item.ItemName.EndsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.ItemDescription.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.ItemDescription.Contains(value, StringComparison.OrdinalIgnoreCase)
                    || item.ItemDescription.EndsWith(value, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(item);
                }
            }
            if (string.IsNullOrEmpty(value))
            {
                ItemCollection = new ObservableCollection<Item>(ItemsList);
            }
            ItemCollection = new ObservableCollection<Item>(list);
        }
        [ObservableProperty]
        string emptyQuantity;
        partial void OnItemQuantityChanged(string? value)
        {
            if(int.TryParse(value, out var quantity) && !string.IsNullOrEmpty(value))
            {
                if(quantity <= 0)
                {
                    return;
                }
                Price = quantity * p;
            }
            if( quantity == 1)
            {
                Price = p;
            }
            if(string.IsNullOrEmpty(value))
            {
                Price = p;
                EmptyQuantity = "Please enter quantity";
            }
            else
            {
                EmptyQuantity = string.Empty;
            }
        }
        [RelayCommand]
        private async Task GoToCart()
        {
          await  App.Current.MainPage.Navigation.PushAsync(new Cart(this));
        }
        [ObservableProperty]
        string? vat;
        [ObservableProperty]
        string? subTotal;
        [RelayCommand]
        private async Task PayAndGo()
        {
            if(TotalOrdered == 0)
            {
              await  App.Current.MainPage.DisplayAlertAsync("Pay and Go", "Cannot proceed with payment while no item has been added to cart","Retry");
                return;
            }
            Vat = (TotalOrdered * 0.15).ToString("C",new CultureInfo("en-ZA"));
            SubTotal = (TotalOrdered - (TotalOrdered * 0.15)).ToString("C", new CultureInfo("en-ZA"));
            await App.Current.MainPage.Navigation.PushAsync(new PayAndGoo(this));
        }
        private async void LoadAisles()
        {
            var l = await _aislleService.GetAislesAsync();
            if(!(l.Count > 0))
            {
                return;
            }
            List<Aisle> list = l.Select(x => new Aisle
            {
                AsileId = x.Id,
                AisleName = x.Aisle_Name,
                AisleDescription = x.Description,
                ImageSourceUrl = x.ImageUrl,
            }).ToList();
            if (list != null)
            {
                AisleCollection = new ObservableCollection<Aisle>(list);
                AislesList = list;
                return;
            }
        }
        [RelayCommand]
        private async Task GetSelectedtItemsForAisle()
        {
            if (SelectedAisle == null)
                return;
            List<Item>? list = await ReturnItemsList();
            if (list.Count > 0 )
            {
                list = list.Select(x => x.AisleId == SelectedAisle.AsileId ? x : null).Where(x => x != null).ToList()!;
                choosedAisleId = SelectedAisle.AsileId;
                if (!(list.Count > 0))
                {
                    ItemCollection = new ObservableCollection<Item>();
                    if (ItemCollection.Count > 0)
                    {
                        ItemCollection.Clear();
                    }
                    EmptyAisle = "There are no items available on this aisle right now";
                    await App.Current.MainPage.Navigation.PushAsync(new AddItemsToCart(this));
                    SelectedAisle = null;
                    return;
                }
                ItemCollection = new ObservableCollection<Item>(list);
                ItemsList = list;
                await App.Current.MainPage.Navigation.PushAsync(new AddItemsToCart(this));
                SelectedAisle = null;
                return;
            }
        }
        [RelayCommand]
        private async Task LoadItems()
        {
            List<Item> list = await ReturnItemsList();
                ItemCollection = new ObservableCollection<Item>(list);
                ItemsList = list;
                return;
        }
        [RelayCommand]
        private async Task AddToCart()
        {
            if (string.IsNullOrEmpty(ItemQuantity) || !int.TryParse(ItemQuantity, out int _))
                return;
            Quantity += int.Parse(ItemQuantity);
            ItemToBeBought.RequestedQuantity = int.Parse(ItemQuantity);
            TotalOrdered += ItemToBeBought.RequestedQuantity * ItemToBeBought.ItemPrice;
            OrderedItems.Add(ItemToBeBought);
            ItemQuantity = "1";
            ItemToBeBought = new Item();
            await App.Current.MainPage.Navigation.PopAsync();
        }
        [RelayCommand]
        private async Task DeleteItem(Item item)
        {
            TotalOrdered -= item.ItemPrice * item.RequestedQuantity;
            Quantity -= item.RequestedQuantity;
            OrderedItems.Remove(item);
            await App.Current.MainPage.Navigation.PopAsync();
            await  App.Current.MainPage.Navigation.PushAsync(new Cart(this));
        }
        [RelayCommand]
        private async Task ChangeItem(Item item)
        {
            ItemName = item.ItemName;
            ItemImageUrl = item.ImageSource;
            Price = item.ItemPrice;
            Discription = item.ItemDescription;
            ItemQuantity = item.RequestedQuantity.ToString();
            TotalOrdered -= item.ItemPrice * item.RequestedQuantity;
            Quantity -= item.RequestedQuantity;
            ItemToBeBought = item;
            OrderedItems.Remove(item);
            await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PushAsync(new ItemPage(this));
        }
        [RelayCommand]
        private async Task CaShPayment()
        {
            double vat = TotalOrdered * 0.15;
            double subtotal = TotalOrdered - vat;
            OrderGenarater orderGenarater = new OrderGenarater(_orderService,_itemService);
           var order = await _orderService.CreateOrderAsync(await orderGenarater.CreatedBROrder(OrderedItems));
            Order o = new()
            {
                OrderId = order.Id,
                OrderDate = order.Occured_On,
                OrderedItemsQty = order.Quantity,
                Code = order.Code,
                OrderNumber = order.Order_Number,
                TotalAmount = order.Price,
            };
            await _dbBoughtItemService.CreateItemAsync(await orderGenarater.DbBoughtItems(OrderedItems, o));
            await _SignalROrderService.SendOrder(await orderGenarater.CreateSignalROrder(o));
            await orderGenarater.UpdateQuantityLeft(OrderedItems);
            Quantity = 0;
            TotalOrdered = 0.00;
            ItemQuantity = "1";
            OrderedItems.Clear();
            SubTotal = 0.ToString("C",new CultureInfo("en-ZA"));
            Vat = 0.ToString("C", new CultureInfo("en-ZA"));
            string invoic = await orderGenarater.CreateInvoice(OrderedItems, TotalOrdered, subtotal, vat, o);
            await App.Current.MainPage.DisplayAlertAsync("Slip", invoic, "Ok");
            await  App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PopAsync();
        }
        [RelayCommand]
        private async Task CardPayment()
        {
            double vat = TotalOrdered * 0.15;
            double subtotal = TotalOrdered - vat;
            OrderGenarater orderGenarater = new OrderGenarater(_orderService, _itemService);
            var order = await _orderService.CreateOrderAsync(await orderGenarater.CreatedBROrder(OrderedItems));
            Order o = new()
            {
                OrderId = order.Id,
                OrderDate = order.Occured_On,
                OrderedItemsQty = order.Quantity,
                Code = order.Code,
                OrderNumber = order.Order_Number,
                TotalAmount = order.Price,
            };
            await _dbBoughtItemService.CreateItemAsync(await orderGenarater.DbBoughtItems(OrderedItems, o));
            await _SignalROrderService.SendOrder(await orderGenarater.CreateSignalROrder(o));
            await orderGenarater.UpdateQuantityLeft(OrderedItems);
            Quantity = 0;
            TotalOrdered = 0.00;
            ItemQuantity = "1";
            OrderedItems.Clear();
            SubTotal = 0.ToString("C", new CultureInfo("en-ZA"));
            Vat = 0.ToString("C", new CultureInfo("en-ZA"));
            string invoic = await orderGenarater.CreateInvoice(OrderedItems, TotalOrdered, subtotal, vat, o);
            await App.Current.MainPage.DisplayAlertAsync("Slip", invoic, "Ok");
            await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PopAsync();
            await App.Current.MainPage.Navigation.PopAsync();
        }
        [ObservableProperty]
        string? itemName;
        [ObservableProperty]
        ImageSource? itemImageUrl;
        [ObservableProperty]
        double? price;
        [ObservableProperty]
        string? discription;
        [RelayCommand]
        private async Task SeleCtedItem()
        {
            if (SelectedItem == null)
                return;
            ItemName = SelectedItem.ItemName;
            Discription = SelectedItem.ItemDescription;
            Price = SelectedItem.ItemPrice;
            p = (double)Price;
            ItemImageUrl = SelectedItem.ImageSource;
            ItemToBeBought = SelectedItem;
            SelectedItem = null;
            await App.Current.MainPage.Navigation.PushAsync(new ItemPage(this));
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
        private void RealTimeAisleUpdate(QuickPickSignlaRService.Models.Aisle aisle)
        {
            Aisle a = new Aisle
            {
                AsileId = aisle.AsileId,
                AisleName = aisle.AisleName,
                AisleDescription = aisle.AisleDescription,
                ImageSourceUrl = aisle.ImageSourceUrl,
            };
            MainThread.InvokeOnMainThreadAsync( () =>
            {
                AisleCollection.Add(a);
            });
        }
        private void DeleteAisle(QuickPickSignlaRService.Models.Aisle aisle)
        {
           Aisle a = new Aisle
            {
                AsileId = aisle.AsileId,
                AisleName = aisle.AisleName,
                AisleDescription = aisle.AisleDescription,
                ImageSourceUrl = aisle.ImageSourceUrl,
            };
            a = AisleCollection.Where(x => x.AsileId ==  aisle.AsileId).FirstOrDefault() ?? new Aisle();
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                AisleCollection.Remove(a);
            });
        }
        private void UpdateAisle(QuickPickSignlaRService.Models.Aisle aisle)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Aisle ais = AisleCollection.Where(x => x.AsileId == aisle.AsileId).FirstOrDefault() ?? new Aisle();
                AisleCollection.Remove(ais);
                ais.AisleName = aisle.AisleName;
                ais.AisleDescription = aisle.AisleDescription;
                ais.ImageSourceUrl = aisle.ImageSourceUrl;
                AisleCollection.Add(ais);
            });
        }
        private void DeleteItemWhenDeleted(QuickPickSignlaRService.Models.Item item)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Item i = ItemCollection.Where(x => x.ItemId == item.ItemId).FirstOrDefault() ?? new Item();
                ItemCollection.Remove(i);
                ItemCollection = new ObservableCollection<Item>(ItemCollection);
            });
        }
        private void ChangeItemWhenChanged(QuickPickSignlaRService.Models.Item item)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Item i = ItemCollection.Where(x => x.ItemId == item.ItemId).FirstOrDefault() ?? new Item();
                ItemCollection.Remove(i);
                i.ItemId = item.ItemId;
                i.AisleId = item.AisleId;
                i.ItemName = item.ItemName;
                i.ItemDescription = item.ItemDescription;
                i.ItemPrice = item.ItemPrice;
                i.ItemQuantity = item.ItemQuantity;
                i.ImageSource = item.ImageSource;
                ItemCollection.Add(i);
                ItemCollection = new ObservableCollection<Item>(ItemCollection);
            });
        }
        private void AddRealTimeItem(QuickPickSignlaRService.Models.Item item)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Item i = new()
                {
                    ItemId = item.ItemId,
                    AisleId = item.AisleId,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ItemPrice = item.ItemPrice,
                    ItemQuantity = item.ItemQuantity,
                    ImageSource = item.ImageSource,
                };
                ItemCollection.Add(i);
                ItemCollection = new ObservableCollection<Item>(ItemCollection);
            });
        }
    }
}

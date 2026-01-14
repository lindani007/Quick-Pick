using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick.QuickPickEmployer.Views;
using QuickPick.QuieckPickCustomer.Models;
using QuickPick.QuieckPickCustomer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace QuickPick.QuieckPickCustomer.ViewModels
{
    public partial class ChooseAiselViewModel : ObservableObject
    {
        private const string file = "items.json";
        List<Aisle> AislesList = new List<Aisle>();
        List<Item> ItemsList = new List<Item>();
        [ObservableProperty]
        List<Item> orderedItems = new List<Item>();
        string choosedAisle;
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
        public ChooseAiselViewModel()
        {
            LoadAisles();
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
                if (item.ItemName.StartsWith(value, StringComparison.OrdinalIgnoreCase)
                    || item.ItemName.Contains(value, StringComparison.OrdinalIgnoreCase)
                    || item.AisleName.EndsWith(value, StringComparison.OrdinalIgnoreCase)
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
                Price = quantity * price;
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
        private const string aislesfile = "Aislesfile.json";
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
            Vat = (TotalOrdered * 0.15).ToString("C",new CultureInfo("en-ZA"));
            SubTotal = (TotalOrdered - (TotalOrdered * 0.15)).ToString("C", new CultureInfo("en-ZA"));
            await App.Current.MainPage.Navigation.PushAsync(new PayAndGoo(this));
        }
        private async Task LoadAisles()
        {
            string fullPath = Path.Combine(FileSystem.AppDataDirectory, aislesfile);

            var json = File.ReadAllText(fullPath);
            if (string.IsNullOrWhiteSpace(json)) return;

            List<Aisle>? list = null;

            try
            {
                // try array first
                list = JsonSerializer.Deserialize<List<Aisle>>(json);
            }
            catch (JsonException)
            {
                list = null;
            }

            if (list != null)
            {
                //convert bytes -> ImageSource for each aisle
                foreach (var a in list)
                {
                    if (a.AisleImageUrl != null && a.AisleImageUrl.Length > 0)
                    {
                        a.ImageSourceUrl = ImageSource.FromStream(() => new MemoryStream(a.AisleImageUrl));
                    }
                    else
                    {
                        a.ImageSourceUrl = ImageSource.FromFile("empty.webp");
                    }
                }

                AisleCollection = new ObservableCollection<Aisle>(list);
                AislesList = list;
                return;
            }

            try
            {
                // fallback: single object
                var single = JsonSerializer.Deserialize<Aisle>(json);
                if (single != null)
                {
                    if (single.AisleImageUrl != null && single.AisleImageUrl.Length > 0)
                        single.ImageSourceUrl = ImageSource.FromStream(() => new MemoryStream(single.AisleImageUrl));
                    else
                        single.ImageSourceUrl = ImageSource.FromFile("empty.webp");

                    AisleCollection = new ObservableCollection<Aisle> { single };
                }
            }
            catch (JsonException)
            {

            }
        }
        [RelayCommand]
        private async Task GetSelectedtForItemsAisle()
        {
            if (SelectedAisle == null)
                return;
            string fullPath = Path.Combine(FileSystem.AppDataDirectory, file);
            List<Item>? list = new List<Item>();
            var json = File.ReadAllText(fullPath);
            if (string.IsNullOrWhiteSpace(json))
            {
                EmptyAisle = "There are no items available on this aisle right now";
                await App.Current.MainPage.Navigation.PushAsync(new AddItemsToCart(this));
                SelectedAisle = null;
                return;
            }

            try
            {
                // try array first
                list = JsonSerializer.Deserialize<List<Item>>(json);
            }
            catch (JsonException)
            {
                list = null;
            }

            if (list.Count > 0 )
            {
                list = list.Select(x => x.AisleName == SelectedAisle.AisleName ? x : null).Where(x => x != null).ToList()!;
                choosedAisle = SelectedAisle.AisleName;
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
                //convert bytes -> ImageSource for each aisle
                foreach (var a in list)
                {
                    if (a.ItemImageUrl != null && a.ItemImageUrl.Length > 0)
                    {
                        a.ImageSource = ImageSource.FromStream(() => new MemoryStream(a.ItemImageUrl));
                    }
                    else
                    {
                        a.ImageSource = ImageSource.FromFile("empty.webp");
                    }
                }

                ItemCollection = new ObservableCollection<Item>(list);
                ItemsList = list;
                await App.Current.MainPage.Navigation.PushAsync(new AddItemsToCart(this));
                choosedAisle = SelectedAisle.AisleName;
                SelectedAisle = null;
                return;
            }

            try
            {
                // fallback: single object
                var single = JsonSerializer.Deserialize<Item>(json);
                if (single != null)
                {
                    if (single.ItemImageUrl != null && single.ItemImageUrl.Length > 0)
                        single.ImageSource = ImageSource.FromStream(() => new MemoryStream(single.ItemImageUrl));
                    else
                        single.ImageSource = ImageSource.FromFile("empty.webp");

                    ItemCollection = new ObservableCollection<Item> { single };
                }
            }
            catch (JsonException)
            {

            }
            await App.Current.MainPage.Navigation.PushAsync(new AddItemsToCart(this));
            SelectedAisle = null;
        }
        [RelayCommand]
        private async Task LoadItems()
        {
            string fullPath = Path.Combine(FileSystem.AppDataDirectory, file);
            var json = File.ReadAllText(fullPath);
            if (string.IsNullOrWhiteSpace(json)) return;
            List<Item>? list = null;
            try
            {
                // try array first
                list = JsonSerializer.Deserialize<List<Item>>(json);
            }
            catch (JsonException)
            {
                list = null;
            }
            if (list.Count > 0)
            {
                list = list.Select(x => x.AisleName == choosedAisle ? x : null).Where(x => x != null).ToList()!;
                //convert bytes -> ImageSource for each aisle
                foreach (var a in list)
                {
                    if (a.ItemImageUrl != null && a.ItemImageUrl.Length > 0)
                    {
                        a.ImageSource = ImageSource.FromStream(() => new MemoryStream(a.ItemImageUrl));
                    }
                    else
                    {
                        a.ImageSource = ImageSource.FromFile("empty.webp");
                    }
                }
                ItemCollection = new ObservableCollection<Item>(list);
                ItemsList = list;
                return;
            }
            try
            {
                // fallback: single object
                var single = JsonSerializer.Deserialize<Item>(json);
                if (single != null)
                {
                    if (single.ItemImageUrl != null && single.ItemImageUrl.Length > 0)
                        single.ImageSource = ImageSource.FromStream(() => new MemoryStream(single.ItemImageUrl));
                    else
                        single.ImageSource = ImageSource.FromFile("empty.webp");
                    ItemCollection = new ObservableCollection<Item> { single };
                }
            }
            catch (JsonException)
            {
            }
        }
        [RelayCommand]
        private async Task AddToCart()
        {
            if (string.IsNullOrEmpty(ItemQuantity) || !int.TryParse(ItemQuantity, out int _))
                return;
            Quantity += int.Parse(itemQuantity);
            ItemToBeBought.ItemQuantity = int.Parse(itemQuantity);
            TotalOrdered += ItemToBeBought.ItemQuantity * ItemToBeBought.ItemPrice;
            OrderedItems.Add(ItemToBeBought);
            ItemQuantity = "1";
            ItemToBeBought = new Item();
            await App.Current.MainPage.Navigation.PopAsync();
        }
        [RelayCommand]
        private async Task DeleteItem(Item item)
        {
            TotalOrdered -= item.ItemPrice * item.ItemQuantity;
            Quantity -= item.ItemQuantity;
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
            ItemQuantity = item.ItemQuantity.ToString();
            TotalOrdered -= item.ItemPrice * item.ItemQuantity;
            Quantity -= item.ItemQuantity;
            ItemToBeBought = item;
            OrderedItems.Remove(item);
            await App.Current.MainPage.Navigation.PushAsync(new ItemPage(this));
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
    }
}

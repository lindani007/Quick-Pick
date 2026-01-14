using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick_Employer.QuickPickEmployer.Models;
using QuickPick_Employer.QuickPickEmployer.ViewModel.ItemsClasses;
using QuickPick_Employer.QuickPickEmployer.Views;
using QuickPickBlobService;
using QuickPickDBApiService.Services;
using QuickPickSignlaRService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel
{
    public partial class ItemViewModel : ObservableObject
    {
        //Collections
        List<Aisle> AislesList = new List<Aisle>();
        List<Item> ItemsToSearchOn = new List<Item>();
        [ObservableProperty]
        ObservableCollection<Aisle> aisleCollection = new ObservableCollection<Aisle>();
        [ObservableProperty]
        ObservableCollection<Item> itemCollection = new ObservableCollection<Item>();
        List<Item> itemsList = new List<Item>();

        //Properties
        string choosedAisle;
        int choosedId;
        [ObservableProperty]
        private string searchAisle;
        [ObservableProperty]
        private string? searchItem;
        [ObservableProperty]
        private string? emptyItem;
        [ObservableProperty]
        private string? itemName;
        [ObservableProperty]
        private string? itemDescription;
        [ObservableProperty]
        private string itemPrice;
        [ObservableProperty]
        private int aisleId;
        [ObservableProperty]
        private ImageSource itemImageUrl = "empty.webp";
        [ObservableProperty]
        private string quantity;
        [ObservableProperty]
        private string heading;
        [ObservableProperty]
        string? emptyItems;
        [ObservableProperty]
        private string? itemNameError;
        [ObservableProperty]
        private string? itemDescriptionError;
        [ObservableProperty]
        private string? itemPriceError;
        [ObservableProperty]
        private string? itemImageUrlError;
        [ObservableProperty]
        private string? quantityError;
        [ObservableProperty]
        private Color? itemNameColor;
        [ObservableProperty]
        private Color? itemDescriptionColor;
        [ObservableProperty]
        private Color? itemPriceColor;
        [ObservableProperty]
        private Color? itemImageUrlColor;
        [ObservableProperty]
        private Color? quantityColor;
        byte[] imageByte;
        string blobImageUrl;

        //Selected Items
        [ObservableProperty]
        private Aisle selectedAisle;
        [ObservableProperty]
        private Item selectedItem;

        //Services and other variables
        ItemService _iemService;
        StockService _stockService;
        AisleService _aisleService;
        private bool UpdatingItem = false;
        Item ItemToUpdate;
        ViewModelAisle _vmAisle;
        SignlaRAisleService _signalRAisleService;
        SignalRItemService _signalRItemService;
        ImageStorege _imageStorege;
        public ItemViewModel(ItemService itemService, AisleService aisleService, ViewModelAisle vmAisle,
            SignlaRAisleService signalRAisleService, SignalRItemService signalRItemService, StockService stockService,ImageStorege imageStorege)
        {
            ItemImageUrl = "empty.webp";
            _iemService = itemService;
            Task.Run(async () => await GetItems()).Wait();
            _stockService = stockService;
            _aisleService = aisleService;
            _vmAisle = vmAisle;
            _signalRAisleService = signalRAisleService;
            _signalRItemService = signalRItemService;
            _imageStorege = imageStorege;
            StartConnection();
        }
        private async void StartConnection()
        {
            await _signalRItemService.ConnectSignlaR();
        }
        private async Task GetItems()
        {
            var l = await _iemService.GetItemsAsync();
            itemsList = l.Select(propa => new Item
            {
                ItemId = propa.ID,
                ItemName = propa.Item_Name,
                ItemDescription = propa.Description,
                ItemPrice = propa.Price,
                ItemQuantity = propa.Quantity,
                AisleId = propa.Aisle_Id,
                ImageSource = propa.ImageUrl,
            }).ToList();
        }
        partial void OnItemNameChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ItemNameError = "Item name cannot be null or empty";
                ItemNameColor = Colors.Red;
            }
            else if (value.Length < 3)
            {
                ItemNameError = "Item name must be at least 3 characters long";
                ItemNameColor = Colors.Red;
            }
            else
            {
                ItemNameError = string.Empty;
                ItemNameColor = Colors.Grey;
            }
        }
        partial void OnItemDescriptionChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ItemDescriptionError = "Item description cannot be null or empty";
                ItemDescriptionColor = Colors.Red;
            }
            else if (value.Length < 10)
            {
                ItemDescriptionError = "Item description must be at least 10 characters long";
                ItemDescriptionColor = Colors.Red;
            }
            else
            {
                ItemDescriptionError = string.Empty;
                ItemDescriptionColor = Colors.Grey;
            }
        }
        partial void OnItemPriceChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ItemPriceError = "Item price cannot be empty";
                ItemPriceColor = Colors.Red;
            }
            else if (!double.TryParse(value.ToString(), out double _))
            {
                ItemPriceError = "Item price must be a valid number";
                ItemPriceColor = Colors.Red;
            }
            else if (double.Parse(value) <= 0)
            {
                ItemPriceError = "Item price must be greater than zero";
                ItemPriceColor = Colors.Red;
            }
            else if (double.Parse(value) > 10000)
            {
                ItemPriceError = "Item price seems too high";
                ItemPriceColor = Colors.Red;
            }
            else
            {
                ItemPriceError = string.Empty;
                ItemPriceColor = Colors.Grey;
            }
        }
        partial void OnQuantityChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                QuantityError = "Item quantity cannot be empty";
                QuantityColor = Colors.Red;
            }
            else if (int.TryParse(value.ToString(), out int result) == false)
            {
                QuantityError = "Item quantity must be a valid number";
                QuantityColor = Colors.Red;
            }
            else if (int.Parse(value) < 0)
            {
                QuantityError = "Item quantity cannot be negative";
                QuantityColor = Colors.Red;
            }
            else
            {
                QuantityError = string.Empty;
                QuantityColor = Colors.Grey;
            }
        }
        partial void OnItemImageUrlChanged(ImageSource value)
        {
            bool isDefaultImage = ItemImageUrl == null
                      || (ItemImageUrl.ToString()?.Contains("empty.webp", StringComparison.OrdinalIgnoreCase) == true);
            if (isDefaultImage)
            {
                ItemImageUrlError = "Image is Required";
                ItemImageUrlColor = Colors.Red;
            }
            else
            {
                ItemImageUrlError = string.Empty;
                ItemImageUrlColor = Colors.Grey;
            }
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
                    if (!string.IsNullOrEmpty(EmptyItems))
                    {
                        EmptyItems = string.Empty;
                    }
                    list.Add(item);
                }
            }
            if (!(list.Count > 0))
            {
                EmptyItems = "Aisle with that name was not found";
            }
            if (string.IsNullOrEmpty(value))
            {
                EmptyItems = string.Empty;
                AisleCollection = new ObservableCollection<Aisle>(AislesList);
            }
            AisleCollection = new ObservableCollection<Aisle>(list);
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
                    if (!string.IsNullOrEmpty(EmptyItems))
                    {
                        EmptyItems = string.Empty;
                    }
                    list.Add(item);
                }
            }
            if (!(list.Count > 0))
            {
                EmptyItems = "Item with that name was not found";
            }
            if (string.IsNullOrEmpty(value))
            {
                EmptyItems = string.Empty;
                ItemCollection = new ObservableCollection<Item>(ItemsToSearchOn);
            }
            ItemCollection = new ObservableCollection<Item>(list);
        }

        private bool ValidaUserInput()
        {
            bool isValid = true;
            bool isDefaultImage = ItemImageUrl == null
                                  || (ItemImageUrl.ToString()?.Contains("empty.webp", StringComparison.OrdinalIgnoreCase) == true);
            if (string.IsNullOrWhiteSpace(ItemName)
                && string.IsNullOrWhiteSpace(ItemDescription)
                && string.IsNullOrWhiteSpace(ItemPrice)
                && string.IsNullOrWhiteSpace(Quantity)
                && isDefaultImage)
            {
                ItemNameError = "Item name cannot be null or empty";
                ItemNameColor = Colors.Red;
                ItemDescriptionError = "Item description cannot be null or empty";
                ItemDescriptionColor = Colors.Red;
                ItemPriceError = "Item price cannot be empty";
                ItemPriceColor = Colors.Red;
                QuantityError = "Item quantity cannot be empty";
                QuantityColor = Colors.Red;
                ItemImageUrlError = "Image is Required";
                ItemImageUrlColor = Colors.Red;
                return isValid = false;
            }
            else if (string.IsNullOrWhiteSpace(ItemName))
            {
                ItemNameError = "Item name cannot be null or empty";
                ItemNameColor = Colors.Red;
                return isValid = false;
            }
            else if (ItemName.Length < 3)
            {
                ItemNameError = "Item name must be at least 3 characters long";
                ItemNameColor = Colors.Red;
                return isValid = false;
            }
            else if (string.IsNullOrWhiteSpace(ItemDescription))
            {
                ItemDescriptionError = "Item description cannot be null or empty";
                ItemDescriptionColor = Colors.Red;
                return isValid = false;
            }
            else if (ItemDescription.Length < 10)
            {
                ItemDescriptionError = "Item description must be at least 10 characters long";
                ItemDescriptionColor = Colors.Red;
                return isValid = false;
            }
            else if (string.IsNullOrWhiteSpace(ItemPrice))
            {
                ItemPriceError = "Item price cannot be empty";
                ItemPriceColor = Colors.Red;
                return isValid = false;
            }
            else if (!double.TryParse(ItemPrice.ToString(), out double result))
            {
                ItemPriceError = "Item price must be a valid number";
                ItemPriceColor = Colors.Red;
                return isValid = false;
            }
            else if (double.Parse(ItemPrice) <= 0)
            {
                ItemPriceError = "Item price must be greater than zero";
                ItemPriceColor = Colors.Red;
                return isValid = false;
            }
            else if (double.Parse(ItemPrice) > 10000)
            {
                ItemPriceError = "Item price seems too high";
                ItemPriceColor = Colors.Red;
                return isValid = false;
            }
            else if (string.IsNullOrWhiteSpace(Quantity))
            {
                QuantityError = "Item quantity cannot be empty";
                QuantityColor = Colors.Red;
                return isValid = false;
            }
            else if (int.TryParse(Quantity.ToString(), out int _) == false)
            {
                QuantityError = "Item quantity must be a valid number";
                QuantityColor = Colors.Red;
                return isValid = false;
            }
            else if (int.Parse(Quantity) <= 0)
            {
                QuantityError = "Item quantity cannot be negative";
                QuantityColor = Colors.Red;
                return isValid = false;
            }
            else if (isDefaultImage)
            {
                ItemImageUrlError = "Image is Required";
                ItemImageUrlColor = Colors.Red;
                return isValid = false;
            }
            else
            {
                QuantityError = string.Empty;
                QuantityColor = Colors.Grey;
                ItemPriceError = string.Empty;
                ItemPriceColor = Colors.Grey;
                ItemDescriptionError = string.Empty;
                ItemDescriptionColor = Colors.Grey;
                ItemNameError = string.Empty;
                ItemNameColor = Colors.Grey;
                ItemImageUrlError = string.Empty;
                ItemImageUrlColor = Colors.Grey;
            }
            return isValid;
        }
        [RelayCommand]
        private async Task AddItem()
        {
            if (ValidaUserInput())
            {
                QuickPickDBApiService.Models.ApiModels.Stock stock = new()
                {
                    Item_Name = ItemName,
                    Quantity = int.Parse(Quantity),
                    Aisle_Id = AisleId,
                    Description = ItemDescription,
                    Price = double.Parse(ItemPrice),
                    ImageUrl = blobImageUrl,
                };
                QuickPickDBApiService.Models.ApiModels.Item i = new()
                {
                    Aisle_Id = AisleId,
                    Item_Name = ItemName,
                    Description = ItemDescription,
                    Price = double.Parse(ItemPrice),
                    Quantity = int.Parse(Quantity),
                    ImageUrl = blobImageUrl,
                };
                if (UpdatingItem)
                {
                    i.ID = ItemToUpdate.ItemId;
                    await _iemService.UpdateItemAsync(i);
                    QuickPickSignlaRService.Models.Item signalrItem = new()
                    {
                        ItemId = i.ID,
                        ItemName = i.Item_Name,
                        ItemDescription = i.Description,
                        ItemPrice = i.Price,
                        ItemQuantity = i.Quantity,
                        ImageSource = i.ImageUrl,
                        AisleId = i.Aisle_Id,
                    };
                    await _signalRItemService.UpdateItem(signalrItem);
                    UpdatingItem = false;
                    ItemName = string.Empty;
                    ItemDescription = string.Empty;
                    ItemPrice = string.Empty;
                    Quantity = string.Empty;
                    ItemImageUrl = "empty.webp";
                    await App.Current.MainPage.Navigation.PopAsync();
                    LoadItemsCommand.Execute(null);
                    return;
                }
                await _stockService.AddStock(stock);
                await UpdateItem(i);
                ItemName = string.Empty;
                ItemDescription = string.Empty;
                ItemPrice = string.Empty;
                Quantity = string.Empty;
                ItemImageUrl = "empty.webp";
            }
        }
        [RelayCommand]
        async Task GetImage()
        {
            var photo = await MediaPicker.PickPhotosAsync(); 
            var pickedFile = photo.FirstOrDefault();
            if (pickedFile == null)
            {
                ItemImageUrl = ImageSource.FromFile("empty.webp");
                return;
            }

            using var stream = await pickedFile.OpenReadAsync();
            using var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();
            using var openStream = new MemoryStream( bytes);
            blobImageUrl = await _imageStorege.UploadImageAsync(openStream, pickedFile.FileName);
            ItemImageUrl = ImageSource.FromStream(() => new MemoryStream(bytes));
            return;
        }
        [RelayCommand]
        private async Task GetSelectedtAisle()
        {
            if (SelectedAisle == null)
                return;

            AisleId = SelectedAisle.AsileId;
            choosedId = SelectedAisle.AsileId;
            try
            {
                Heading = "Add Items For " + SelectedAisle.AisleName + " Aisle";
                await App.Current.MainPage.Navigation.PushAsync(new AddedItems(this));
            }
            catch
            {

            }
            SelectedAisle = null;
        }
        [RelayCommand]
        private async Task GetSelectedtForItemsAisle(Aisle aisle)
        {
            AisleId = aisle.AsileId;
            List<Item>? list = new List<Item>();
            if (list != null)
            {
                list = itemsList.Where(x => x.AisleId == aisle.AsileId).ToList()!;
                if (!(list.Count > 0))
                {
                    if((ItemCollection.Count > 1))
                    {
                        ItemCollection.Clear();
                        ItemsToSearchOn.Clear();
                    }
                    EmptyItems = "No items that has been added for this aisle";
                    await App.Current.MainPage.Navigation.PushAsync(new ItemsPage(this));
                    return;
                }
                EmptyItems = string.Empty;
                ItemCollection = new ObservableCollection<Item>(list);
                ItemsToSearchOn = list;
                await App.Current.MainPage.Navigation.PushAsync(new ItemsPage(this));
                return;
            }

        }
        [RelayCommand]
        private async Task GetSelectedtItem()
        {
            if (SelectedItem == null)
                return;

            ItemName = SelectedItem.ItemName;
            ItemDescription = SelectedItem.ItemDescription;
            ItemImageUrl = "empty.webp"; //SelectedItem.ImageSource;
            ItemPrice = SelectedItem.ItemPrice.ToString();
            Quantity = SelectedItem.ItemQuantity.ToString();
            try
            {
                await App.Current.MainPage.Navigation.PushAsync(new AddItems(this));
            }
            catch
            {

            }
            SelectedAisle = null;
        }
        [RelayCommand]
        private async Task AddNewItem(Item i)
        {
            try
            {
                if (i.ImageSource is UriImageSource uriImageSource && uriImageSource.Uri != null)
                {
                    i.ImageUrl = uriImageSource.Uri.ToString();
                }
                else
                {
                    i.ImageUrl = null;
                }
                ItemName = i.ItemName;
                ItemDescription = i.ItemDescription;
                ItemImageUrl = i.ImageSource;
                ItemPrice = i.ItemPrice.ToString();
                await App.Current.MainPage.Navigation.PushAsync(new AddItems(this));
            }
            catch (Exception ex)
            {
                //Error = ex.Message;
            }
        }
        [RelayCommand]
        private async Task DeleteItem(Item i)
        {
            try
            {
                await _iemService.DeleteItemAsync(i.ItemId);
                ItemCollection.Remove(i);
                QuickPickSignlaRService.Models.Item item = new()
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ItemDescription = i.ItemDescription,
                    ItemPrice = i.ItemPrice,
                    ItemQuantity = i.ItemQuantity,
                    ImageSource = i.ImageUrl,
                    AisleId = i.AisleId,
                };
                await _signalRItemService.DeleteItem(item);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlertAsync("Error", ex.Message, "Retry");
            }
        }
        [RelayCommand]
        private async Task ChangeItem(Item i)
        {
            try
            {
                ItemToUpdate = new Item();
                ItemToUpdate = i;
                if (i.ImageSource is UriImageSource uriImageSource && uriImageSource.Uri != null)
                {
                    ItemToUpdate.ImageUrl = uriImageSource.Uri.ToString();
                }
                else
                {
                    ItemToUpdate.ImageUrl = null;
                }
                UpdatingItem = true;
                ItemCollection.Remove(i);
                ItemName = i.ItemName;
                ItemDescription = i.ItemDescription;
                ItemImageUrl = i.ImageSource;
                ItemPrice = i.ItemPrice.ToString();
                Quantity = i.ItemQuantity.ToString();
                await App.Current.MainPage.Navigation.PushAsync(new AddItems(this));
            }
            catch (Exception ex)
            {
                //Error = ex.Message;
            }
        }

        [RelayCommand]
        private async Task LoadAisles()
        {
            List<Aisle>? list = null;
            var l = await _aisleService.GetAislesAsync();
            list = l.Select(propa => new Aisle
            {
                AsileId = propa.Id,
                AisleName = propa.Aisle_Name,
                AisleDescription = propa.Description,
                ImageSourceUrl = propa.ImageUrl,
            }).ToList();
            AisleCollection = new ObservableCollection<Aisle>(list);
            AislesList = list;
        }
        [RelayCommand]
        private async Task LoadItems()
        {
            var l = await _iemService.GetItemsAsync();
            itemsList = l.Select(propa => new Item
            {
                ItemId = propa.ID,
                ItemName = propa.Item_Name,
                ItemDescription = propa.Description,
                ItemPrice = propa.Price,
                ItemQuantity = propa.Quantity,
                AisleId = propa.Aisle_Id,
                ImageSource = propa.ImageUrl,
            }).ToList();
            List<Item>? list = null;
            if (list != null)
            {
                list = itemsList.Select(x => x.AisleId == choosedId ? x : null).Where(x => x != null).ToList()!;
                ItemCollection = new
                    ObservableCollection<Item>(list);
                return;
            }
        }
        [RelayCommand]
        private async Task DeleteAisle(Aisle aisle)
        {
            try
            {
                await _aisleService.DeleteAisleAsync(aisle.AsileId);
                AisleCollection.Remove(aisle);
                QuickPickSignlaRService.Models.Aisle a = new()
                {
                    AsileId = aisle.AsileId,
                    AisleName = aisle.AisleName,
                    AisleDescription = aisle.AisleDescription,
                };
                await _signalRAisleService.DeleteAisle(a);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlertAsync("Items", ex.Message, "Retry");
            }
        }

        [RelayCommand]
        private async Task ChangeAisle(Aisle aisle)
        {
            try
            {
                _vmAisle.IsAisleBeingUpdated = true;
                _vmAisle.AisleToUpdate = new Aisle();
                _vmAisle.AisleToUpdate = aisle;
                if (_vmAisle.AisleToUpdate.ImageSourceUrl is UriImageSource uri && uri != null)
                {
                    _vmAisle.AisleToUpdate.ImageUrl = uri.Uri.ToString();
                }
                _vmAisle.AisleName = aisle.AisleName;
                _vmAisle.AisleDescription = aisle.AisleDescription;
                _vmAisle.AisleImageUrl = aisle.ImageSourceUrl;
                await App.Current.MainPage.Navigation.PushAsync(new AddAisles(_vmAisle));
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlertAsync("Items", ex.Message, "Retry");
            }
        }
        [RelayCommand]
        private async Task GoAddItems()
        {
            await App.Current.MainPage.Navigation.PushAsync(new AddItems(this));
        }
        private async Task UpdateItem(QuickPickDBApiService.Models.ApiModels.Item item)
        {
             var list = await _iemService.GetItemsAsync();
            bool itemExists = list.Any(i => i.Item_Name == item.Item_Name);
            if(itemExists)
            {
                var i = list.Where(i => i.Item_Name == item.Item_Name).FirstOrDefault();
                i.Quantity = (item.Quantity + i.LeftQuantity);
                i.LeftQuantity += item.Quantity;
                await _iemService.UpdateItemAsync(i);
            }
            else
            {
                item.LeftQuantity = item.Quantity;
                await _iemService.CreateItemAsync(item);
            }
        }
    }
}

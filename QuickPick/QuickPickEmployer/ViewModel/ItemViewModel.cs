using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick.QuickPickEmployer.Models;
using QuickPick.QuickPickEmployer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickPick.QuickPickEmployer.ViewModel
{
    public partial class ItemViewModel : ObservableObject
    {

        private const string file = "items.json";
        List<Aisle> AislesList = new List<Aisle>();
        string choosedAisle;
        [ObservableProperty]
        private string searchAisle;
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
        private string aisleName;
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
        [ObservableProperty]
        ObservableCollection<Aisle> aisleCollection = new ObservableCollection<Aisle>();
        [ObservableProperty]
        ObservableCollection<Item> itemCollection = new ObservableCollection<Item>();
        [ObservableProperty]
        private Aisle selectedAisle;
        [ObservableProperty]
        private Item selectedItem;
        public ItemViewModel()
        {
            ItemImageUrl = @"C:\Users\user\source\repos\QuickPick\QuickPick\Resources\Images\empty.webp";
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
            foreach(var item in AislesList)
            {
                if(item.AisleName.StartsWith(value, StringComparison.OrdinalIgnoreCase) 
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
            if(!(list.Count > 0))
            {
                EmptyItems = "Aisle with that name was not found";
            }
            if(string.IsNullOrEmpty(value))
            {
                EmptyItems = string.Empty;
                AisleCollection = new ObservableCollection<Aisle>( AislesList);
            }
            AisleCollection = new ObservableCollection<Aisle>(list);
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
        int id = 0;
        [RelayCommand]
        private async Task AddItem()
        {
            if (ValidaUserInput())
            {
                if (imageByte == null || imageByte.Length == 0)
                {
                    var bytes = await ImageHelpers.ImageSourceToBytesAsync(ItemImageUrl);
                    imageByte = bytes;
                }
                id++;
                Item item = new Item
                {
                    ItemId = id,
                    ItemName = ItemName,
                    ItemDescription = ItemDescription,
                    ItemPrice = double.Parse(ItemPrice),
                    ItemQuantity = int.Parse(Quantity),
                    ItemImageUrl = imageByte,
                    AisleId = AisleId,
                    AisleName = AisleName,
                };
                string filePath = Path.Combine(FileSystem.AppDataDirectory, ItemViewModel.file);
                string jsonData = JsonSerializer.Serialize(item);


                List<Item> list = new List<Item>();

                if (File.Exists(filePath))
                {
                    var existing = await File.ReadAllTextAsync(filePath);
                    var trimmed = existing?.Trim();
                    if (string.IsNullOrEmpty(trimmed))
                    {
                        list = new List<Item>();
                    }
                    else
                    {
                        try
                        {
                            char first = trimmed[0];
                            if (first == '[')
                            {
                                list = JsonSerializer.Deserialize<List<Item>>(trimmed) ?? new List<Item>();
                            }
                            else if (first == '{')
                            {
                                var single = JsonSerializer.Deserialize<Item>(trimmed);
                                list = single != null ? new List<Item> { single } : new List<Item>();
                            }
                            else
                            {
                                list = new List<Item>();
                            }
                        }
                        catch (JsonException)
                        {
                            list = new List<Item>();
                        }
                    }
                    list.Add(item);
                    string json = JsonSerializer.Serialize(list);
                    await File.WriteAllTextAsync(filePath, json);
                    ItemName = string.Empty;
                    ItemDescription = string.Empty;
                    ItemPrice = string.Empty;
                    Quantity = string.Empty;
                    ItemImageUrl = "empty.webp";
                }
            }
        }
        [RelayCommand]
        async Task GetImage()
        {
            var pickedFile = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select An Item Image",
                FileTypes = FilePickerFileType.Images,
            });

            if (pickedFile == null)
            {
                ItemImageUrl = ImageSource.FromFile("empty.webp");
                return;
            }

            using var stream = await pickedFile.OpenReadAsync();
            using var mem = new MemoryStream();
            await stream.CopyToAsync(mem);
            var bytes = mem.ToArray();
            imageByte = bytes;
            ItemImageUrl = ImageSource.FromStream(() => new MemoryStream(bytes));
            return;
            ItemImageUrl = @"C:\Users\user\source\repos\QuickPick\QuickPick\Resources\Images\empty.webp";
        }
        [RelayCommand]
        private async Task GetSelectedtAisle()
        {
            if (SelectedAisle == null)
                return;

            AisleId = SelectedAisle.AsileId;
            AisleName = SelectedAisle.AisleName;
            try
            {
                Heading = "Add Items For " + SelectedAisle.AisleName + " Aisle";
                await App.Current.MainPage.Navigation.PushAsync(new AddItems(this));
            }
            catch
            {

            }
            SelectedAisle = null;
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
                EmptyItems = "No items available for this aisle.";
                await App.Current.MainPage.Navigation.PushAsync(new ItemsPage(this));
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

            if (list != null)
            {
                list = list.Select(x => x.AisleName == SelectedAisle.AisleName ? x : null).Where(x => x != null).ToList()!;
                choosedAisle = SelectedAisle.AisleName;
                if (!(list.Count > 0))
                {
                    EmptyItems = "No items available for this aisle.";
                    ItemCollection = new ObservableCollection<Item>();
                    if (ItemCollection.Count > 0)
                    {
                        ItemCollection.Clear();
                    }
                    await App.Current.MainPage.Navigation.PushAsync(new ItemsPage(this));
                    return;
                }
                EmptyItems = string.Empty;
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
                await App.Current.MainPage.Navigation.PushAsync(new ItemsPage(this));
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
            await App.Current.MainPage.Navigation.PushAsync(new ItemsPage(this));
            SelectedAisle = null;
        }
        [RelayCommand]
        private async Task GetSelectedtItem()
        {
            if (SelectedItem == null)
                return;

            ItemName = SelectedItem.ItemName;
            ItemDescription = SelectedItem.ItemDescription;
            ItemImageUrl = SelectedItem.ImageSource;
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
        private async Task DeleteAisle(Item i)
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, file);
                var jsonData = await File.ReadAllTextAsync(filePath);
                var list = JsonSerializer.Deserialize<List<Item>>(jsonData) ?? new List<Item>();
                foreach (var item in list)
                {
                    if (item.ItemName == i.ItemName)
                    {
                        list.Remove(item);
                        break;
                    }
                }
                ItemCollection.Remove(i);
                string json = JsonSerializer.Serialize(list);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                //Error = ex.Message;
            }
        }
        [RelayCommand]
        private async Task ChangeAisle(Item i)
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, file);
                var jsonData = await File.ReadAllTextAsync(filePath);
                var list = JsonSerializer.Deserialize<List<Item>>(jsonData) ?? new List<Item>();
                foreach (var item in list)
                {
                    if (item.ItemName == i.ItemName)
                    {
                        list.Remove(item);
                        break;
                    }
                }
                ItemCollection.Remove(i);
                string json = JsonSerializer.Serialize(list);
                await File.WriteAllTextAsync(filePath, json);
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

        private const string aislesfile = "Aislesfile.json";
        [RelayCommand]
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
            if (list != null)
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
    }
}

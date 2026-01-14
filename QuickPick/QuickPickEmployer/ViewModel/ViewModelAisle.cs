using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick.QuickPickEmployer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace QuickPick.QuickPickEmployer.ViewModel
{
    public partial class ViewModelAisle : ObservableObject
    {
        private const string file = "Aislesfile.json";
        // Aisle properties
        [ObservableProperty]
        private string? aisleName;
        [ObservableProperty]
        private string? aisleDescription;
        [ObservableProperty]
        ImageSource? aisleImageUrl = "empty.webp";
        [ObservableProperty]
        private Aisle selectedAisle;
        [ObservableProperty]
        private string error;
        [ObservableProperty]
        private ObservableCollection<Aisle> aisles;

        // Error messages
        [ObservableProperty]
        string? aisleNameError;
        [ObservableProperty]
        string? aisleDescriptionError;
        [ObservableProperty]
        string? aisleImageUrlError;

        // Colors for entry fields
        [ObservableProperty]
        Color? aisleDescriptionEntryColor;
        [ObservableProperty]
        Color? aisleNameColor;
        [ObservableProperty]
        Color? aisleImageColor;
        byte[]? imageBytes;
        partial void OnAisleNameChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                AisleNameError = "Aisle name cannot be null or empty";
                AisleNameColor = Colors.Red;
            }
            else if (value.Length < 3)
            {
                AisleNameError = "Aisle name must be at least 3 characters long";
                AisleNameColor = Colors.Red;
            }
            else
            {
                AisleNameError = string.Empty;
                AisleNameColor = Colors.Grey;
            }
        }
        partial void OnAisleDescriptionChanged(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                AisleDescriptionError = "Aisle description cannot be null or empty";
                AisleDescriptionEntryColor = Colors.Red;
            }
            else if (value.Length < 10)
            {
                AisleDescriptionError = "Aisle description must be at least 10 characters long";
                AisleDescriptionEntryColor = Colors.Red;
            }
            else
            {
                AisleDescriptionError = string.Empty;
                AisleDescriptionEntryColor = Colors.Grey;
            }
        }
        partial void OnAisleImageUrlChanged(ImageSource? value)
        {
            bool isDefaultImage = value == null
                                 || (value.ToString()?.Contains("empty.webp", StringComparison.OrdinalIgnoreCase) == true);
            if (isDefaultImage)
            {
                AisleImageUrlError = "Please select an image for the aisle.";
                AisleImageColor = Colors.Red;
            }
            else
            {
                AisleImageUrlError = string.Empty;
                AisleImageColor = Colors.Grey;
            }
        }
        int id = 1;
        [RelayCommand]
        async Task AddAisle()
        {
            try
            {
                if (!ValidateUserInput()) return;
                id++;
                var aisle = new Aisle
                {
                    AsileId = id,
                    AisleName = AisleName,
                    AisleDescription = AisleDescription,
                    AisleImageUrl = imageBytes
                };
                var a = new Aisle
                {
                    AsileId = id,
                    AisleName = AisleName,
                    AisleDescription = AisleDescription,
                    ImageSourceUrl = AisleImageUrl
                };

                Aisles = new ObservableCollection<Aisle>();
                var filePath = Path.Combine(FileSystem.AppDataDirectory, file);

                List<Aisle> list = new List<Aisle>();

                if (File.Exists(filePath))
                {
                    var existing = await File.ReadAllTextAsync(filePath);
                    var trimmed = existing?.Trim();
                    if (string.IsNullOrEmpty(trimmed))
                    {
                        list = new List<Aisle>();
                    }
                    else
                    {
                        try
                        {
                            char first = trimmed[0];
                            if (first == '[')
                            {
                                list = JsonSerializer.Deserialize<List<Aisle>>(trimmed) ?? new List<Aisle>();
                            }
                            else if (first == '{')
                            {
                                var single = JsonSerializer.Deserialize<Aisle>(trimmed);
                                list = single != null ? new List<Aisle> { single } : new List<Aisle>();
                            }
                            else
                            {
                                list = new List<Aisle>();
                            }
                        }
                        catch (JsonException)
                        {
                            list = new List<Aisle>();
                        }
                    }
                }

                if (await IsUnique(aisle))
                {
                    // persist to disk as an array (always)
                    list.Add(aisle);
                    string json = JsonSerializer.Serialize(list);
                    await File.WriteAllTextAsync(filePath, json);

                    // Ensure UI updates happen on main thread. Also create ImageSource on UI thread
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        // create a fresh ImageSource bound for UI consumption
                        if (a.AisleImageUrl == null && aisle.AisleImageUrl != null)
                        {
                            a.ImageSourceUrl = ImageSource.FromStream(() => new MemoryStream(aisle.AisleImageUrl));
                        }
                        else if (a.ImageSourceUrl == null)
                        {
                            a.ImageSourceUrl = ImageSource.FromFile("empty.webp");
                        }
                        Aisles.Add(a);
                    });

                    // reset inputs
                    AisleName = " ";
                    AisleDescription = " ";
                    AisleImageUrl = "empty.webp";
                    return;
                }
                else
                {
                    Error = "Aisle Already Exist";
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        [RelayCommand]
        async Task GetImage()
        {
            try
            {
                var pickedFile = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (pickedFile == null)
                {
                    AisleImageUrl = ImageSource.FromFile("empty.webp");
                    return;
                }
                byte[] originalBytes;
                using (var s = await pickedFile.OpenReadAsync())
                using (var ms = new MemoryStream())
                {
                    await s.CopyToAsync(ms);
                    originalBytes = ms.ToArray();
                }
                byte[] resizedBytes = await Task.Run(() =>
                {
                    try
                    {
                        // Decode original
                        using var srcStream = new MemoryStream(originalBytes);
                        var original = SkiaSharp.SKBitmap.Decode(srcStream);
                        if (original == null)
                            return originalBytes;

                        // Target max dimension (adjust to desired size)
                        const int maxDimension = 1024;
                        var width = original.Width;
                        var height = original.Height;
                        var scale = Math.Min(1.0, (double)maxDimension / Math.Max(width, height));
                        int newW = (int)Math.Max(1, Math.Round(width * scale));
                        int newH = (int)Math.Max(1, Math.Round(height * scale));
                        var resizedBitmap = original;
                        if (scale < 1.0)
                        {
                            resizedBitmap = original.Resize(new SkiaSharp.SKImageInfo(newW, newH), SkiaSharp.SKFilterQuality.Medium)
                                             ?? original;
                        }
                        using var image = SkiaSharp.SKImage.FromBitmap(resizedBitmap);
                        using var encoded = image.Encode(SkiaSharp.SKEncodedImageFormat.Jpeg, 80); // quality 80
                        return encoded.ToArray();
                    }
                    catch
                    {
                        return originalBytes;
                    }
                });
                imageBytes = resizedBytes;
                AisleImageUrl = ImageSource.FromStream(() => new MemoryStream(resizedBytes));
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        [RelayCommand]
        private async Task DeleteAisle(Aisle aisle)
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, file);
                var jsonData = await File.ReadAllTextAsync(filePath);
                var list = JsonSerializer.Deserialize<List<Aisle>>(jsonData) ?? new List<Aisle>();
                foreach (var item in list)
                {
                    if (item.AisleName == aisle.AisleName)
                    {
                        list.Remove(item);
                        break;
                    }
                }
                Aisles.Remove(aisle);
                string json = JsonSerializer.Serialize(list);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        [RelayCommand]
        private async Task ChangeAisle(Aisle aisle)
        {
            try
            {
                var filePath = Path.Combine(FileSystem.AppDataDirectory, file);
                var jsonData = await File.ReadAllTextAsync(filePath);
                var list = JsonSerializer.Deserialize<List<Aisle>>(jsonData) ?? new List<Aisle>();
                AisleName = aisle.AisleName;
                AisleDescription = aisle.AisleDescription;
                AisleImageUrl = aisle.ImageSourceUrl;
               foreach (var item in list)
                {
                    if (item.AisleName == aisle.AisleName)
                    {
                       list.Remove(item);
                        break;
                    }
                }
                Aisles.Remove(aisle);
                string json = JsonSerializer.Serialize(list);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
        [RelayCommand]
        private async Task UloadAisles()
        {
            Aisles = new ObservableCollection<Aisle>();
            if (Aisles.Count > 0)
            {
                Aisles.Clear();
            }
             
        }
        private bool ValidateUserInput()
        {
            bool isDefaultImage = AisleImageUrl == null
                                 || (AisleImageUrl.ToString()?.Contains("empty.webp", StringComparison.OrdinalIgnoreCase) == true);
            bool valid = true;
            if (string.IsNullOrEmpty(AisleName) && string.IsNullOrEmpty(AisleDescription) && isDefaultImage)
            {
                AisleNameError = "Aisle name cannot be null or empty";
                AisleNameColor = Colors.Red;
                AisleDescriptionError = "Aisle description cannot be null or empty";
                AisleDescriptionEntryColor = Colors.Red;
                AisleImageUrlError = "Please select an image for the aisle.";
                AisleImageColor = Colors.Red;
                valid = false;
            }
            else if (string.IsNullOrEmpty(AisleName))
            {
                AisleNameError = "Aisle name cannot be null or empty";
                AisleNameColor = Colors.Red;
                valid = false;
            }
            else if (AisleName.Length < 3)
            {
                AisleNameError = "Aisle name must be at least 3 characters long";
                AisleNameColor = Colors.Red;
                valid = false;
            }
            if (string.IsNullOrEmpty(AisleDescription))
            {
                AisleDescriptionError = "Aisle description cannot be null or empty";
                AisleDescriptionEntryColor = Colors.Red;
                valid = false;
            }
            else if (AisleDescription.Length < 10)
            {
                AisleDescriptionError = "Aisle description must be at least 10 characters long";
                AisleDescriptionEntryColor = Colors.Red;
                valid = false;
            }
            else if (AisleImageUrl == null)
            {
                AisleImageUrlError = "Please select an image for the aisle.";
                AisleImageColor = Colors.Red;
                valid = false;
            }
            else
            {
                AisleNameError = string.Empty;
                AisleNameColor = Colors.Grey;
                AisleDescriptionError = string.Empty;
                AisleDescriptionEntryColor = Colors.Grey;
                AisleImageUrlError = string.Empty;
                AisleImageColor = Colors.Grey;
            }
            return valid;
        }
        private async Task<bool> IsUnique(Aisle aisle)
        {
            bool isUnique = true;
            var list = await ReturnAisle();
            foreach (var item in list)
            {
                if (item.AisleName == aisle.AisleName)
                {
                    return isUnique = false;
                }
            }
            return isUnique;
        }
        private async Task<List<Aisle>> ReturnAisle()
        {
            var filePath = Path.Combine(FileSystem.AppDataDirectory, file);
            var existing = await File.ReadAllTextAsync(filePath);
            var list = new List<Aisle>();
            var trimmed = existing?.Trim();
            if (string.IsNullOrEmpty(trimmed))
            {
              return  list = new List<Aisle>();
            }
            else
            {
                try
                {
                    char first = trimmed[0];
                    if (first == '[')
                    {
                       return list = JsonSerializer.Deserialize<List<Aisle>>(trimmed) ?? new List<Aisle>();
                    }
                    else if (first == '{')
                    {
                        var single = JsonSerializer.Deserialize<Aisle>(trimmed);
                       return list = single != null ? new List<Aisle> { single } : new List<Aisle>();
                    }
                    else
                    {
                        // unknown root token -> create new list (or log/throw)
                       return list = new List<Aisle>();

                    }

                }
                catch (JsonException)
                {
                    // Optionally log `existing` here for diagnosis
                   return list = new List<Aisle>();
                }
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickPick_Employer.QuickPickEmployer.Models;
using QuickPickDBApiService.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using QuickPickSignlaRService.Services;
using QuickPickBlobService;

namespace QuickPick_Employer.QuickPickEmployer.ViewModel
{
    public partial class ViewModelAisle : ObservableObject
    {
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

        string imageUrlFromBlob;
        public Aisle AisleToUpdate;
        public bool IsAisleBeingUpdated = false;
        AisleService _aisleService;
        SignlaRAisleService _signalRAisleService;
        ImageStorege _imageStorege;
        public ViewModelAisle(AisleService aisleService, SignlaRAisleService signlaRAisleService, ImageStorege imageStorege)
        {
            _aisleService = aisleService;
            _signalRAisleService = signlaRAisleService;
            _imageStorege = imageStorege;
            StartConnection();
        }
        public async void StartConnection()
        {
            await _signalRAisleService.ConnectSignlaR();
        }
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
        [RelayCommand]
        async Task AddAisle()
        {
                if (!ValidateUserInput()) return;
                if(IsAisleBeingUpdated)
                {
                        QuickPickSignlaRService.Models.Aisle ais = new()
                        {
                            AsileId = AisleToUpdate.AsileId,
                            AisleName = AisleName,
                            AisleDescription = AisleDescription,
                            ImageSourceUrl = AisleToUpdate.ImageUrl,
                        };
                        await _signalRAisleService.UpdateAisle(ais);
                        QuickPickDBApiService.Models.ApiModels.Aisle asl = new()
                        {
                            Id = AisleToUpdate.AsileId,
                            Aisle_Name = AisleName,
                            Description = AisleDescription,
                            ImageUrl = AisleToUpdate.ImageUrl,
                         };
                     await   _aisleService.UpdateAisleAsync(asl);
                AisleName = string.Empty;
                AisleDescription = string.Empty;
                AisleImageUrl = "empty.webp";
                              await  App.Current.MainPage.Navigation.PopAsync();
                return;
                }
                var a = new Aisle()
                {
                    AisleName = AisleName,
                    AisleDescription = AisleDescription,
                    ImageUrl = imageUrlFromBlob,
                };
              
                Aisles = new ObservableCollection<Aisle>();
            Aisles.Add(a);
            Aisles = new ObservableCollection<Aisle>(Aisles);
                if (await IsUnique(a))
                {
                        QuickPickDBApiService.Models.ApiModels.Aisle asl = new()
                        {
                            Aisle_Name = a.AisleName,
                            Description = a.AisleDescription,
                            ImageUrl = a.ImageUrl,
                        };
                QuickPickSignlaRService.Models.Aisle signalRAisle = new()
                        {
                            AisleName = a.AisleName,
                            AisleDescription = a.AisleDescription,
                            ImageSourceUrl = a.ImageUrl,
                        };

                        await _aisleService.CreateAisleAsync(asl);
                        var list = await _aisleService.GetAislesAsync();
                         signalRAisle.AsileId = list.Where(x => x.Aisle_Name == signalRAisle.AisleName).Select(x => x.Id).FirstOrDefault();
                         await _signalRAisleService.SendAisle(signalRAisle);
                        // reset inputs
                        AisleName = string.Empty;
                        AisleDescription = string.Empty;
                        AisleImageUrl = "empty.webp";
                        return;
                }
                else
                {
                    Error = "Aisle Already Exist";
                }
        }
            
        [RelayCommand]
        async Task GetImage()
        {
            var pickedFiles = await MediaPicker.PickPhotosAsync();

            var pickedFile = pickedFiles?.FirstOrDefault();
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
            using var uploadStream = new MemoryStream(resizedBytes);
            imageUrlFromBlob = await _imageStorege.UploadImageAsync(uploadStream, pickedFile.FileName);
            AisleImageUrl = ImageSource.FromStream(() => new MemoryStream(resizedBytes));
            try
            {
                
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
                await _aisleService.DeleteAisleAsync(aisle.AsileId);
                Aisles.Remove(aisle);
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
                Error = ex.Message;
            }
        }
        [RelayCommand]
        private async Task ChangeAisle(Aisle aisle)
        {
            try
            {
                IsAisleBeingUpdated = true;
                AisleToUpdate = new Aisle();
                AisleToUpdate = aisle;
                if(AisleToUpdate.ImageSourceUrl is UriImageSource uri && uri != null)
                {
                    AisleToUpdate.ImageUrl = uri.Uri.ToString();
                }
                AisleName = aisle.AisleName;
                AisleDescription = aisle.AisleDescription;
                AisleImageUrl = aisle.ImageSourceUrl;
                Aisles.Remove(aisle);
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
            var list = await _aisleService.GetAislesAsync();
            List<Aisle> l = list.Select(x =>
                new Aisle()
                {
                    AsileId = x.Id,
                    AisleName = x.Aisle_Name,
                    AisleDescription = x.Description,
                    ImageSourceUrl = x.ImageUrl,
                }).ToList();
           return l; 
        }
    }
}

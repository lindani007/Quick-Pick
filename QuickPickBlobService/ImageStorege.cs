using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace QuickPickBlobService
{
    public class ImageStorege
    {
        private readonly HttpClient _client;
        public ImageStorege(string url)
        {
           _client = new HttpClient();
            _client.BaseAddress = new Uri(url);
        }
        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(imageStream), "file", fileName);
            var response = await _client.PostAsync("api/ImageStorer", content);
            response.EnsureSuccessStatusCode();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return "Image upload failed.";
            }
            string jsonrespons = await response.Content.ReadAsStringAsync();
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonrespons))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("blobUrl", out JsonElement blobUrlElement))
                    {
                        return blobUrlElement.GetString() ?? "Image upload failed.";
                    }
                    else
                    {
                        return "Image upload failed.";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Image upload failed.";
            }
        }
    }
}

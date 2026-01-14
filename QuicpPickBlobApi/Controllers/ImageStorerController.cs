using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuicpPickBlobApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageStorerController : ControllerBase
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ImageStorerController(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> StoreImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var containerClient = _blobServiceClient.GetBlobContainerClient("quickpickimages");           
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(file.FileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }
            return Ok(new { BlobUrl = blobClient.Uri.ToString() });
        }
    }
}

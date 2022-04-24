
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Storage.Blobs;
using System.Linq;

namespace Storage_Account_File_Serving
{
    public static class StorageAccountFileServing
    {
        [FunctionName("StorageAccountFileServing")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            string accountName = req.Query["accountname"];
            string filePath = req.Query["filepath"];

            string container = filePath.Split("/").First();
            string fileName = filePath.Split("/",2).Last();


            BlobServiceClient blobServiceClient = new BlobServiceClient(
                new Uri($"https://{accountName}.blob.core.windows.net"),
                new InteractiveBrowserCredential());

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(container);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);
            System.IO.Stream fileStream = blobClient.OpenRead();

            var bytes = new byte[fileStream.Length];
            fileStream.Seek(0, SeekOrigin.Begin);
            await fileStream.ReadAsync(bytes, 0, bytes.Length);
            fileStream.Dispose();

            return new FileContentResult(bytes, "application/octet-stream")
            {
                FileDownloadName = filePath.Split("/").Last()
            };
        }
    }
}


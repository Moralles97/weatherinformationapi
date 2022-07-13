using Microsoft.WindowsAzure.Storage;
using Serilog;

namespace WeatherInformation.Infrastructure.Azure
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        public AzureBlobService(CloudStorageAccount cloudStorageAccount)
        {
            _cloudStorageAccount = cloudStorageAccount;
        }

        public async Task<Stream?> GetItemFromBlobAsync(string containerName, string filePath)
        {
            var container = _cloudStorageAccount.CreateCloudBlobClient()
                                                .GetContainerReference(containerName);

            var blob = container.GetBlobReference(filePath);
            
            if (await blob.ExistsAsync())
            {
                Log.Information($"Requested container: {containerName} and file path: {filePath} found.");

                return await blob.OpenReadAsync();
            }

            Log.Information($"Requested container or file path does not exist on storage.");

            return null;
        }
    }
}

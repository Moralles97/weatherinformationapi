using Microsoft.WindowsAzure.Storage;

namespace WeatherInformation.Infrastructure.Azure
{
    public class AzureBlobService : IAzureBlobService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;

        public AzureBlobService(CloudStorageAccount cloudStorageAccount)
        {
            _cloudStorageAccount = cloudStorageAccount;
        }

        public async Task<string> GetItemFromBlobAsync(string containerName, string filePath)
        {
            var container = _cloudStorageAccount.CreateCloudBlobClient()
                                                .GetContainerReference(containerName);

            var blob = container.GetBlobReference(filePath);
            
            if (await blob.ExistsAsync())
            {
                using var stream = new MemoryStream();

                await blob.DownloadToStreamAsync(stream).ConfigureAwait(false);
               
                stream.Position = 0;
                
                using var streamReader = new StreamReader(stream);
                
                return await streamReader.ReadToEndAsync();
            }

            return string.Empty;
        }
    }
}

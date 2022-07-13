namespace WeatherInformation.Infrastructure.Azure
{
    public interface IAzureBlobService
    {
        Task<Stream?> GetItemFromBlobAsync(string containerName, string filePath);
    }
}

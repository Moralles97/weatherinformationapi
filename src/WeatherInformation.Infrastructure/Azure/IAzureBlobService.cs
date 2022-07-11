namespace WeatherInformation.Infrastructure.Azure
{
    public interface IAzureBlobService
    {
        Task<string> GetItemFromBlobAsync(string containerName, string filePath);
    }
}

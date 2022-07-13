using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;
using NUnit.Framework;
using WeatherInformation.Infrastructure.Azure;

namespace WeatherInformation.UnitTests.Services
{
    public class AzureBlobServiceTests
    {
        private Mock<CloudStorageAccount> _cloudStorageAccountMock;
        private Mock<CloudBlobClient> _cloudStorageBlobClientMock;
        private Mock<CloudBlob> _cloudStorageBlobMock;
        private Mock<CloudBlobContainer> _cloudStorageBlobContainerMock;

        private Uri _uriMock = new("blob:https://bloburlmock.blob");

        private const string _containerNameMock = "container";
        private const string _filePathMock = "filepath";
            
        [SetUp]
        public void Setup() 
        {
            var credentials = new StorageCredentials("accountName", "keyValue", "keyName");
            _cloudStorageAccountMock = new Mock<CloudStorageAccount>(credentials, true);

            _cloudStorageBlobClientMock = new Mock<CloudBlobClient>(_uriMock);
            _cloudStorageBlobMock = new Mock<CloudBlob>(_uriMock);
            _cloudStorageBlobContainerMock = new Mock<CloudBlobContainer>(_uriMock);

            _cloudStorageAccountMock.Setup(x => x.CreateCloudBlobClient())
                                    .Returns(_cloudStorageBlobClientMock.Object);

            _cloudStorageBlobClientMock.Setup(x => x.GetContainerReference(It.IsAny<string>()))
                                       .Returns(_cloudStorageBlobContainerMock.Object);

            _cloudStorageBlobContainerMock.Setup(x => x.GetBlobReference(It.IsAny<string>()))
                                          .Returns(_cloudStorageBlobMock.Object);

            
        }

        [Test(Description = "GetItemFromBlob should return stream if the requested file path exists.")]
        public async Task GetItemFromBlobShouldReturnStreamForExistingFile()
        {
            //arrange
            _cloudStorageBlobMock.Setup(x => x.ExistsAsync())
                                 .ReturnsAsync(true);

            _cloudStorageBlobMock.Setup(x => x.OpenReadAsync())
                                 .ReturnsAsync(new MemoryStream());            

            var service = new AzureBlobService(_cloudStorageAccountMock.Object);

            //act
            var result = await service.GetItemFromBlobAsync(_containerNameMock, _filePathMock);

            //assert
            Assert.NotNull(result);
        }

        [Test(Description = "GetItemFromBlob should return null if the requested file path does not exist.")]
        public async Task GetItemFromBlobShouldReturnNullForNonExistingFile()
        {
            //arrange
            _cloudStorageBlobMock.Setup(x => x.ExistsAsync())
                                 .ReturnsAsync(false);

            var service = new AzureBlobService(_cloudStorageAccountMock.Object);

            //act
            var result = await service.GetItemFromBlobAsync(_containerNameMock, _filePathMock);

            //assert
            Assert.Null(result);
        }
    }
}

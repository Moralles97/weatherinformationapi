using Moq;
using NUnit.Framework;
using WeatherInformation.Application.Services;
using WeatherInformation.Domain.Dto.Request;
using WeatherInformation.Domain.Enums;
using WeatherInformation.Infrastructure.Azure;

namespace WeatherInformation.UnitTests.Services
{
    public class MeasurementDataServiceTests
    {
        private Mock<IAzureBlobService> _azureBlobServiceMock;
        private const string measurementContainer = "iotbackend";

        [SetUp]
        public void Setup()
        {
            _azureBlobServiceMock = new Mock<IAzureBlobService>();
        }

        [Test(Description = "GetDataByDeviceSensorTypeAndDay should request correct file path to azure blob service")]
        public async Task GetDataByDeviceSensorTypeAndDayShouldRequestCorrectFilePath()
        {
            //arrange
            var request = new GetDataRequestDto
            {
                DeviceId = "deviceIdMock",
                Date = DateTime.Now,
                SensorType = SensorType.humidity
            };

            var filePathExpected = $"{request.DeviceId}/{request.SensorType}/{request.Date:yyyy-MM-dd}.csv";

            _azureBlobServiceMock.Setup(x => x.GetItemFromBlobAsync(measurementContainer, filePathExpected))
                                 .ReturnsAsync(new MemoryStream());

            var service = new MeasurementDataService(_azureBlobServiceMock.Object);

            //act
            await service.GetDataByDeviceSensorTypeAndDayAsync(request);

            //assert
            _azureBlobServiceMock.Verify(mock => mock.GetItemFromBlobAsync(measurementContainer, filePathExpected), Times.Once());
        }

        [Test(Description = "GetDataForDeviceAsync should request correct file path to azure blob service")]
        public async Task GetDataForDeviceAsyncShouldRequestCorrectFilePath()
        {
            //arrange
            var request = new GetDataForDeviceRequestDto
            {
                DeviceId = "deviceIdMock",
                Date = DateTime.Now
            };

            var filePathExpectedList = new List<string>();

            foreach (var sensorType in Enum.GetValues(typeof(SensorType)))
                filePathExpectedList.Add($"{request.DeviceId}/{sensorType}/{request.Date:yyyy-MM-dd}.csv");

            _azureBlobServiceMock.Setup(x => x.GetItemFromBlobAsync(measurementContainer, It.IsIn<string>(filePathExpectedList)))
                                 .ReturnsAsync(new MemoryStream());

            var service = new MeasurementDataService(_azureBlobServiceMock.Object);

            //act
            await service.GetDataByDeviceAndDayAsync(request);

            //assert
            foreach (var filePath in filePathExpectedList)
                _azureBlobServiceMock.Verify(mock => mock.GetItemFromBlobAsync(measurementContainer, filePath), Times.Once());
        }

        [Test(Description = "GetDataForDeviceAsync should complete execution even if a file does not exist on azure")]
        public async Task GetDataForDeviceAsyncShouldCompleteForNonExistentFile()
        {
            //arrange
            var request = new GetDataForDeviceRequestDto
            {
                DeviceId = "deviceIdMock",
                Date = DateTime.Now
            };

            var filePathExpectedList = new List<string>();
            
            foreach (var sensorType in Enum.GetValues(typeof(SensorType)))
                filePathExpectedList.Add($"{request.DeviceId}/{sensorType}/{request.Date:yyyy-MM-dd}.csv");

            _azureBlobServiceMock.Setup(x => x.GetItemFromBlobAsync(measurementContainer, filePathExpectedList[0]))
                                 .ReturnsAsync(new MemoryStream());

            _azureBlobServiceMock.Setup(x => x.GetItemFromBlobAsync(measurementContainer, filePathExpectedList[1]))
                                 .ReturnsAsync(() => null);

            _azureBlobServiceMock.Setup(x => x.GetItemFromBlobAsync(measurementContainer, filePathExpectedList[2]))
                                 .ReturnsAsync(new MemoryStream());

            var service = new MeasurementDataService(_azureBlobServiceMock.Object);

            //act
            await service.GetDataByDeviceAndDayAsync(request);

            //assert
            _azureBlobServiceMock.Verify(mock => mock.GetItemFromBlobAsync(measurementContainer, filePathExpectedList[2]), Times.Once());
        }
    }
}

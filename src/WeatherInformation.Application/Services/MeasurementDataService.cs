using WeatherInformation.Application.Contracts;
using WeatherInformation.Domain.Dto.Request;
using WeatherInformation.Domain.Enums;
using WeatherInformation.Domain.Models;
using WeatherInformation.Infrastructure.Azure;
using WeatherInformation.Infrastructure.Extensions;

namespace WeatherInformation.Application.Services
{
    public class MeasurementDataService : IMeasurementDataService
    {
        private readonly IAzureBlobService _azureBlobService;
        private const string _measurementDataContainerName = "iotbackend";

        public MeasurementDataService(IAzureBlobService azureBlobService)
        {
            _azureBlobService = azureBlobService;
        }

        /// <summary>
        /// Gets measurement data by device, sensor type and day.
        /// </summary>
        public async Task<Stream?> GetDataByDeviceSensorTypeAndDayAsync(GetDataRequestDto request)
        {
            string filePath = $"{request.DeviceId}/{request.SensorType}/{request.Date:yyyy-MM-dd}.csv";

            return await _azureBlobService.GetItemFromBlobAsync(_measurementDataContainerName, filePath);
        }

        /// <summary>
        /// Gets compressed measurement data by device and sensor type.
        /// </summary>
        public async Task<Stream?> GetCompressedDataByDeviceAndSensorTypeAsync(GetCompressedDataRequestDto request)
        {
            string filePath = $"{request.DeviceId}/{request.SensorType}/historical.zip";

            return await _azureBlobService.GetItemFromBlobAsync(_measurementDataContainerName, filePath);
        }

        /// <summary>
        /// Gets measurement data by device and day, for all sensor types available.
        /// </summary>
        public async Task<Stream> GetDataByDeviceAndDayAsync(GetDataForDeviceRequestDto request)
        {
            var files = new List<CompressedFile>();
            var tasks = new List<Task>();

            foreach (var sensorType in Enum.GetValues(typeof(SensorType)))
            {
                tasks.Add(Task.Run(async () =>
                {
                    string filePath = $"{request.DeviceId}/{sensorType}/{request.Date:yyyy-MM-dd}.csv";

                    var stream = await _azureBlobService.GetItemFromBlobAsync(_measurementDataContainerName, filePath);

                    if (stream == null) return;

                    files.Add(new CompressedFile
                    {
                        FileName = $"{request.DeviceId}-{sensorType}-{request.Date:yyyy-MM-dd}.csv",
                        FileStream = stream
                    });
                }));
            }

            await Task.WhenAll(tasks);

            return files.CompressToZip();
        }
    }
}

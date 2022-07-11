using WeatherInformation.Application.Contracts;
using WeatherInformation.Domain.Dto.Request;
using WeatherInformation.Domain.Enums;
using WeatherInformation.Infrastructure.Azure;

namespace WeatherInformation.Application.Services
{
    public class MeasurementDataService : IMeasurementDataService
    {
        private readonly IAzureBlobService _azureConnectionService;
        private const string _measurementDataContainerName = "iotbackend";

        public MeasurementDataService(IAzureBlobService azureConnectionService)
        {
            _azureConnectionService = azureConnectionService;
        }

        /// <summary>
        /// Gets measurement data by device, sensor type and day.
        /// </summary>
        public async Task<string> GetDataByDeviceSensorTypeAndDay(GetDataRequestDto request)
        {
            string filePath = $"{request.DeviceId}/{request.SensorType}/{request.Date:yyyy-MM-dd}.csv";

            return await _azureConnectionService.GetItemFromBlobAsync(_measurementDataContainerName, filePath);
        }

        /// <summary>
        /// Gets measurement data by device and day, for all sensor types available.
        /// </summary>
        public async Task<IEnumerable<string>> GetDataByDeviceAndDay(GetDataForDeviceRequestDto request)
        {
            var filesResult = new List<string>();
            var tasks = new List<Task>;

            foreach (var sensorType in Enum.GetValues(typeof(SensorType)))
            {
                tasks.Add(Task.Run(async () =>
                {
                    string filePath = $"{request.DeviceId}/{sensorType}/{request.Date:yyyy-MM-dd}.csv";

                    string fileContent = $"{sensorType};\n";
                    fileContent += await _azureConnectionService.GetItemFromBlobAsync(_measurementDataContainerName, filePath);

                    filesResult.Add(fileContent);
                }));
            }

            await Task.WhenAll(tasks);

            return filesResult;
        }
    }
}

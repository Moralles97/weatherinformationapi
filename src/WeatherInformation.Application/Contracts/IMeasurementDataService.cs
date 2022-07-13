using WeatherInformation.Domain.Dto.Request;

namespace WeatherInformation.Application.Contracts
{
    public interface IMeasurementDataService
    {
        Task<Stream?> GetDataByDeviceSensorTypeAndDayAsync(GetDataRequestDto request);
        Task<Stream?> GetCompressedDataByDeviceAndSensorTypeAsync(GetCompressedDataRequestDto request);
        Task<Stream> GetDataByDeviceAndDayAsync(GetDataForDeviceRequestDto request);
    }
}

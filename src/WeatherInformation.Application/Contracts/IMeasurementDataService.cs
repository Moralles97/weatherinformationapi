using WeatherInformation.Domain.Dto.Request;

namespace WeatherInformation.Application.Contracts
{
    public interface IMeasurementDataService
    {
        Task<string> GetDataByDeviceSensorTypeAndDay(GetDataRequestDto request);
        Task<IEnumerable<string>> GetDataByDeviceAndDay(GetDataForDeviceRequestDto request);
    }
}

using WeatherInformation.Domain.Enums;

namespace WeatherInformation.Domain.Dto.Request
{
    public class GetCompressedDataRequestDto
    {
        public string DeviceId { get; set; }
        public SensorType SensorType { get; set; }  
    }
}

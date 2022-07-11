using WeatherInformation.Domain.Enums;

namespace WeatherInformation.Domain.Dto.Request
{
    public class GetDataRequestDto
    {
        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
        public SensorType SensorType { get; set; }
    }
}
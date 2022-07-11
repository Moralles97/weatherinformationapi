namespace WeatherInformation.Domain.Dto.Request
{
    public class GetDataForDeviceRequestDto
    {
        public string DeviceId { get; set; }
        public DateTime Date { get; set; }
    }
}

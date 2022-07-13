namespace WeatherInformation.Domain.Models
{
    public class CompressedFile
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}

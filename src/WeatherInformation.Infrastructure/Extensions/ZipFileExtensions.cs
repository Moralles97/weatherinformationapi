using System.IO.Compression;
using WeatherInformation.Domain.Models;

namespace WeatherInformation.Infrastructure.Extensions
{
    public static class ZipFileExtensions
    {
        public static Stream CompressToZip(this IEnumerable<CompressedFile> files)
        {
            var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
            {
                foreach (var file in files) archive.AddToFile(file);
            }

            stream.Position = 0;
            return stream;
        }

        private static ZipArchiveEntry AddToFile(this ZipArchive archive, CompressedFile file)
        {
            var entry = archive.CreateEntry(file.FileName, CompressionLevel.Optimal);
            using (var stream = entry.Open())
            {
                file.FileStream.CopyTo(stream);
            }

            return entry;
        }
    }
}

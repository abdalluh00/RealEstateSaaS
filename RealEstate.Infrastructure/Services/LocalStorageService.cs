using Microsoft.Extensions.Configuration;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Infrastructure.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _basePath;
        private readonly string _baseUrl;

        public LocalStorageService(IConfiguration configuration)
        {
            _basePath = configuration["Storage:BasePath"] ?? "wwwroot/uploads";
            _baseUrl = configuration["Storage:BaseUrl"] ?? "http://localhost:5171/uploads";
        }

        public async Task<string> UploadAsync(
            Stream fileStream, string fileName, string contentType)
        {
            var folder = Path.Combine(_basePath, "properties");
            Directory.CreateDirectory(folder);

            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(folder, uniqueName);

            using var file = File.Create(filePath);
            await fileStream.CopyToAsync(file);

            return $"{_baseUrl}/properties/{uniqueName}";
        }

        public Task DeleteAsync(string fileUrl)
        {
            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_basePath, "properties", fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return Task.CompletedTask;
        }
    }
}

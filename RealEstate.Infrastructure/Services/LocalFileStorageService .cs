using Microsoft.AspNetCore.Hosting;
using RealEstate.Application.Common.Interfaces;

namespace RealEstate.Infrastructure.Services
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly string _webRootPath;

        public LocalFileStorageService(IWebHostEnvironment env)
            => _webRootPath = env.WebRootPath;

        public async Task<string> SaveAsync(
            Stream fileStream,
            string fileName,
            string folder,
            CancellationToken ct = default)
        {
            // ── Build full directory path ─────────────────
            var directoryPath = Path.Combine(_webRootPath, "uploads", folder);
            Directory.CreateDirectory(directoryPath);

            // ── Unique file name to avoid collisions ──────
            var uniqueName = $"{Guid.NewGuid():N}_{fileName}";
            var filePath = Path.Combine(directoryPath, uniqueName);

            await using var fs = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(fs, ct);

            // ── Return relative URL ───────────────────────
            return $"/uploads/{folder}/{uniqueName}"
                .Replace("\\", "/");
        }

        public Task DeleteAsync(
            string fileUrl,
            CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return Task.CompletedTask;

            // ── Convert URL to physical path ──────────────
            var relativePath = fileUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.Combine(_webRootPath, relativePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }
    }
}
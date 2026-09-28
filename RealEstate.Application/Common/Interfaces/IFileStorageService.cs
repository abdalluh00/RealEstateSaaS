namespace RealEstate.Application.Common.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveAsync(
            Stream fileStream,
            string fileName,
            string folder,
            CancellationToken ct = default);

        Task DeleteAsync(
            string fileUrl,
            CancellationToken ct = default);
    }
}
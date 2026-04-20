namespace Restaurants.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(Stream file, string fileName);
    string? GetBlobSasUrl(string? blobUrl);
}
using ApplicationCore.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class AzureImageService : IImageService
{
    private readonly string _connectionString;
    
    public AzureImageService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
    }
    
    public async Task<string> Save(string container, IFormFile file)
    {
        var client = new BlobContainerClient(_connectionString, container);
        await client.CreateIfNotExistsAsync();
        client.SetAccessPolicy(PublicAccessType.Blob);
        
        var extension = Path.GetExtension(file.FileName);
        var blobName = Guid.NewGuid() + extension;
        var blobClient = client.GetBlobClient(blobName);

        var blobHttpHeaders = new BlobHttpHeaders();
        blobHttpHeaders.ContentType = file.ContentType;
        
        await blobClient.UploadAsync(file.OpenReadStream(), blobHttpHeaders); ;
        return blobClient.Uri.ToString();
    }

    public async Task Delete(string? route, string container)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            return;
        }
        
        var client = new BlobContainerClient(_connectionString, container);
        await client.CreateIfNotExistsAsync();
        var fileName = Path.GetFileName(route);
        var blobClient = client.GetBlobClient(fileName);
        await blobClient.DeleteIfExistsAsync();
    }
}
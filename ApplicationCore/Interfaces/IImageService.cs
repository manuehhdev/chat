using Microsoft.AspNetCore.Http;

namespace ApplicationCore.Interfaces;

public interface IImageService
{
    Task<string> Save(string container, IFormFile file);
    Task Delete(string? route, string container);

    async Task<string> Edit(string? route, string container, IFormFile file)
    {
        await Delete(route, container);
        return await Save(container, file);
    }
}
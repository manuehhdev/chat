using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class LocalImageService : IImageService
{
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalImageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        _env = env;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> Save(string container, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var nombreArchivo = $"{Guid.NewGuid()}{extension}";
        string folder = Path.Combine(_env.WebRootPath, container);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        string ruta = Path.Combine(folder, nombreArchivo);

        using (var ms = new MemoryStream()) 
        {
            await file.CopyToAsync(ms);
            var contenido = ms.ToArray();
            await File.WriteAllBytesAsync(ruta, contenido);
        }

        var request = _httpContextAccessor.HttpContext!.Request;

        var url = $"{request.Scheme}://{request.Host}";
        var urlArchivo = Path.Combine(url, container, nombreArchivo).Replace("\\", "/");
        return urlArchivo;
    }

    public Task Delete(string? route, string container)
    {
        if(string.IsNullOrWhiteSpace(route))
        {
            return Task.CompletedTask;
        }

        var fileName = Path.GetFileName(route);
        var fileDirectory = Path.Combine(_env.WebRootPath, container, fileName);

        if (File.Exists(fileDirectory)) 
        { 
            File.Delete(fileDirectory);
        }

        return Task.CompletedTask;
    }
}
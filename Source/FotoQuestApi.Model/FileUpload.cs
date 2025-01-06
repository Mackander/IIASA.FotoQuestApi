using Microsoft.AspNetCore.Http;

namespace FotoQuestApi.Web.Models;
public record FileUpload
{
    public IFormFile UploadedFile { get; init; }
}
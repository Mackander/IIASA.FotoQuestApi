using Microsoft.AspNetCore.Http;

namespace IIASA.FotoQuestApi.Web.Models;
public record FileUpload
{
    public IFormFile UploadedFile { get; init; }
}
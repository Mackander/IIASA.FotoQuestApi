using Microsoft.AspNetCore.Http;

namespace IIASA.FotoQuestApi.Web.Models
{
    public class FileUpload
    {
        public IFormFile UploadedFile { get; set; }
    }
}

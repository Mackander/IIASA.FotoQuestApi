using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.Database;
using IIASA.FotoQuestApi.FileSystem;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using IIASA.FotoQuestApi.Web.Models;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Web
{
    public interface IImageCoordinator
    {
        public Task<FilePersistanceSuccessResponse> ProcessImage(FileUpload fileUpload);
        public Task<byte[]> GetImage(string fileId, int imageSize);
    }

    public class ImageCoordinator : IImageCoordinator
    {
        private readonly IDbPersistanceProvider DbPersistanceProvider;
        private readonly IFilePersistanceProvider filePersistanceProvider;
        private readonly ImageConfigration imageConfigration;

        public ImageCoordinator(IDbPersistanceProvider persistanceProvider,
                                IFilePersistanceProvider filePersistanceProvider,
                                ImageConfigration imageConfigration)
        {
            this.DbPersistanceProvider = persistanceProvider;
            this.filePersistanceProvider = filePersistanceProvider;
            this.imageConfigration = imageConfigration;
        }

        public async Task<byte[]> GetImage(string fileId, int imageSize)
        {
            Validate(fileId, imageSize);
            return await filePersistanceProvider.GetFileAsync(fileId, new System.Drawing.Size(imageSize, imageSize));
        }

        private void Validate(string fileId, int imageSize)
        {
            if (string.IsNullOrEmpty(fileId?.Trim()))
            {
                throw new BadRequestException($"fileId not provided");
            }
            if (imageSize <= imageConfigration.MinAllowedSize || imageSize >= imageConfigration.MaxAllowedSize)
            {
                throw new BadRequestException($"Provided Image Size '{imageSize}' should be in Range of {imageConfigration.MinAllowedSize} to {imageConfigration.MaxAllowedSize}");
            }
        }

        public async Task<FilePersistanceSuccessResponse> ProcessImage(FileUpload fileUpload)
        {
            if (fileUpload.UploadedFile == null)
            {
                throw new BadRequestException("File not provided");
            }

            var fileData = await filePersistanceProvider.SaveFile(fileUpload);
            DbPersistanceProvider.SaveImageData(fileData);
            return new FilePersistanceSuccessResponse
            {
                Id = fileData.Id
            };
        }
    }
}

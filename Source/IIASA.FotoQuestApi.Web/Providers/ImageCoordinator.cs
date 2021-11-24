using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.Database;
using IIASA.FotoQuestApi.FileSystem;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using IIASA.FotoQuestApi.Web.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Web
{
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
            return await filePersistanceProvider.GetFileAsync(fileId.Trim(), new System.Drawing.Size(imageSize, imageSize));
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

        public async Task<FilePersistanceSuccessResponse> PersistImage(FileUpload fileUpload)
        {
            ValidateFile(fileUpload);

            var fileData = await filePersistanceProvider.SaveFile(fileUpload);
            DbPersistanceProvider.SaveImageData(fileData);
            return new FilePersistanceSuccessResponse
            {
                Id = fileData.Id
            };
            
        }
        private void ValidateFile(FileUpload fileUpload)
        {
            if (fileUpload.UploadedFile == null)
            {
                throw new BadRequestException("File not provided");
            }

            string ext = Path.GetExtension(fileUpload.UploadedFile.FileName)[1..];
            if (!imageConfigration.ValidImageExtensions.Contains<string>(ext, StringComparer.OrdinalIgnoreCase))
            {
                throw new BadRequestException($"Allowed file extensions are : {string.Join(", ", imageConfigration.ValidImageExtensions)}");
            }
        }
    }
}

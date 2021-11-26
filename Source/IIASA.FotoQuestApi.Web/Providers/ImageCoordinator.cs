using IIASA.FotoQuestApi.Configuration;
using IIASA.FotoQuestApi.Database;
using IIASA.FotoQuestApi.FileSystem;
using IIASA.FotoQuestApi.Model;
using IIASA.FotoQuestApi.Model.Exceptions;
using IIASA.FotoQuestApi.Web.Models;
using SixLabors.ImageSharp;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Web
{
    public class ImageCoordinator : IImageCoordinator
    {
        private readonly IDbPersistanceProvider dbPersistanceProvider;
        private readonly IFilePersistanceProvider filePersistanceProvider;
        private readonly ImageConfigration imageConfigration;

        public ImageCoordinator(IDbPersistanceProvider dbPersistanceProvider,
                                IFilePersistanceProvider filePersistanceProvider,
                                ImageConfigration imageConfigration)
        {
            this.dbPersistanceProvider = dbPersistanceProvider;
            this.filePersistanceProvider = filePersistanceProvider;
            this.imageConfigration = imageConfigration;
        }

        public async Task<byte[]> GetImage(string fileId, int imageSize)
        {
            ValidateFileRequest(fileId, imageSize);

            var fileIdTrim = fileId.Trim();
            var fileData = await dbPersistanceProvider.LoadImageData(fileIdTrim);
            fileData.Id = fileIdTrim;
            return await filePersistanceProvider.GetFileAsync(fileData, new Size(imageSize, imageSize));
        }

        private void ValidateFileRequest(string fileId, int imageSize)
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
            ValidateFilePersistRequest(fileUpload);

            var fileData = await filePersistanceProvider.SaveFile(fileUpload);
            dbPersistanceProvider.SaveImageData(fileData);
            return new FilePersistanceSuccessResponse
            {
                Id = fileData.Id
            };
            
        }
        private void ValidateFilePersistRequest(FileUpload fileUpload)
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

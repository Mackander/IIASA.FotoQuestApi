using IIASA.FotoQuestApi.Model;
using System;
using System.Threading.Tasks;

namespace IIASA.FotoQuestApi.Database
{
    public interface IDbPersistanceProvider
    {
        public Task<FileData> LoadImageData(string fileId);
        public void SaveImageData(FileData data);

    }

    public class DbPersistanceProvider : IDbPersistanceProvider
    {
        private readonly IDatabaseProvider databaseProvider;

        public DbPersistanceProvider(IDatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }
        public async Task<FileData> LoadImageData(string fileId)
        {
            return await databaseProvider.LoadImageData(new GetImageDataRequest { Id = fileId });
        }

        public void SaveImageData(FileData data)
        {
            databaseProvider.SaveImageData(new StoreImageDataRequest
            {
                Id = data.Id,
                OriginalName = data.OriginalName,
                DateOfUpload = data.DateOfUpload,
                HorizontalResolution = data.HorizontalResolution,
                VerticalResolution = data.VerticalResolution,
                Height = data.Height,
                Width = data.Width,
                FilePath = data.FilePath,
                FileName = data.FileName,
            });

        }
    }
}
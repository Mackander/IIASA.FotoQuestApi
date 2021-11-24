using IIASA.FotoQuestApi.Model;
using System;

namespace IIASA.FotoQuestApi.Database
{
    public interface IDbPersistanceProvider
    {
        public void LoadImageData();
        public void SaveImageData(FileData data);

    }

    public class DbPersistanceProvider : IDbPersistanceProvider
    {
        private readonly IDatabaseProvider databaseProvider;

        public DbPersistanceProvider(IDatabaseProvider databaseProvider)
        {
            this.databaseProvider = databaseProvider;
        }
        public void LoadImageData()
        {
            throw new NotImplementedException();
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
namespace IIASA.FotoQuestApi.Database
{

    public interface IDatabaseProvider
    {
        public void SaveImageData(IDataRequest data);
    }
}

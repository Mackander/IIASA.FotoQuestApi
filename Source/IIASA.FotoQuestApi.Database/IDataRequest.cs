using System.Data;

namespace IIASA.FotoQuestApi.Database
{
    public interface IDataRequest
    {
        public string Command { get; }
        public CommandType CommandType { get; }
    }
}

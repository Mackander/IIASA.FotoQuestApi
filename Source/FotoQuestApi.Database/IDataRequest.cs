using System.Data;

namespace FotoQuestApi.Database;
public interface IDataRequest
{
    public string Command { get; }
    public CommandType CommandType { get; }
}
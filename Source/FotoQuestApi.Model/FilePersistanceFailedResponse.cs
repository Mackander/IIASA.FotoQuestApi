namespace FotoQuestApi.Model;
public record FilePersistanceFailedResponse
{
    public string Message { get; init; }
    public string Details { get; init; }
}
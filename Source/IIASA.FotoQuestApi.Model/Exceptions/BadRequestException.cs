namespace IIASA.FotoQuestApi.Model.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
namespace CliniCore.Shared.Exceptions;
public class ErrorResponse
{
    public int StatusCode { get; set; }

    public List<ErrorMessage> Errors { get; set; } = new List<ErrorMessage>();

}

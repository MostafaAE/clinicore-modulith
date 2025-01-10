namespace CliniCore.Shared.Exceptions;
internal class ErrorResponse
{
    public int StatusCode { get; set; }

    public List<ErrorMessage> Errors { get; set; } = new List<ErrorMessage>();

}

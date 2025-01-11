namespace CliniCore.Shared.Exceptions;
public abstract class ClinicoreException : Exception
{
    public int StatusCode { get; set; } = 500; // Default status code

    public ClinicoreException()
        : base() { }

    public ClinicoreException(string? message)
        : base(message) { }

    public ClinicoreException(string? message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public ClinicoreException(string? message, Exception innerException)
        : base(message, innerException) { }
}

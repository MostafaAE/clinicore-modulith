using System.Text.Json.Serialization;

namespace CliniCore.Shared.Exceptions;
public class ErrorMessage
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Field { get; set; }

    public string Message { get; set; }
}

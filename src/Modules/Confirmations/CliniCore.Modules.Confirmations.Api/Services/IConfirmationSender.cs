namespace CliniCore.Modules.Confirmations.Api.Services;
public interface IConfirmationSender
{
    Task SendAsync(string receiver, string template);
}

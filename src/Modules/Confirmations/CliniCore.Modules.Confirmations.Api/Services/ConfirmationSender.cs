
using Microsoft.Extensions.Logging;

namespace CliniCore.Modules.Confirmations.Api.Services;
public class ConfirmationSender : IConfirmationSender
{
    private readonly ILogger<ConfirmationSender> _logger;

    public ConfirmationSender(ILogger<ConfirmationSender> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(string receiver, string template)
    {
        _logger.LogInformation($"Sending an email to: '{receiver}', template: '{template}'...");
        await Task.CompletedTask;
    }
}

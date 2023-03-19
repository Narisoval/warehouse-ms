using Infrastructure.MessageBroker.EventBus;
using ILogger = Serilog.ILogger;

namespace Warehouse.API.Messaging;

public class LoggingEventBus : IEventBus
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;

    public LoggingEventBus(IEventBus eventBus,ILogger logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        _logger.Information("Publishing message of type {Type}", typeof(T));
        await _eventBus.PublishAsync(message,cancellationToken);
        _logger.Information("Message {MessageType} published successfully: {@Message}", typeof(T).Name, message);
    }
}
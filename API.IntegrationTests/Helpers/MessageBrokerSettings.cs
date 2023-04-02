namespace API.IntegrationTests.Helpers;

public record MessageBrokerSettings
{
    public Uri Host { get; init; } = null!;

    public string Username { get; init; } = null!;

    public string Password { get; init; } = null!;
}
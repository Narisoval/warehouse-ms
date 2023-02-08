namespace Warehouse.API.DTO;

public record ProviderDto
{
    public Guid ProviderId { get; init; }

    public string CompanyName { get; init; } = "";

    public string PhoneNumber { get; init; } = "";
    
    public string Email { get; init; } = "";
};
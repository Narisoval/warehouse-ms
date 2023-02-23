namespace Warehouse.API.DTO.ProviderDtos;

public record ProviderDto 
{
    public Guid ProviderId { get; set; }

    public string CompanyName { get; init; } = "";

    public string PhoneNumber { get; init; } = "";

    public string Email { get; init; } = "";
};
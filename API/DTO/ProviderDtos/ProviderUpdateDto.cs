namespace Warehouse.API.DTO.ProviderDtos;

public record ProviderUpdateDto
{
    public string CompanyName { get; init; } = "";

    public string PhoneNumber { get; init; } = "";
    
    public string Email { get; init; } = "";
}
﻿using Warehouse.API.DTO.BrandDtos;
using Warehouse.API.DTO.ProviderDtos;

namespace Warehouse.API.DTO.ProductDtos;

public record ProductDto 
{
    public Guid ProductId { get; set; }

    public string Name { get; init; } = "";

    public int Quantity { get; init; } = default;

    public decimal FullPrice { get; init; } = default;

    public string Description { get; init; } = "";

    public IList<ProductImageDto>? Images { get; init; } = null!;

    public decimal Sale { get; init; } = default;


    public bool IsActive { get; init; } = default;

    public string? Category { get; init; }
    
    public ProviderUpdateDto? Provider { get; init; }

    public BrandUpdateDto? Brand { get; init; }
}
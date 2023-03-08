using System.Data;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasConversion(
                productName => productName.Value,
                value => ProductName.From(value).Value);

        builder.Property(p => p.Quantity)
            .HasConversion(
                quantity => quantity.Value,
                value => Quantity.From(value).Value);

        builder.Property(p => p.FullPrice)
            .HasConversion(
                price => price.Value,
                value => Price.From(value).Value);

        builder.Property(p => p.Description)
            .HasConversion(
                description => description.Value,
                value => ProductDescription.From(value).Value);

        builder.Property(p => p.Sale)
            .HasConversion(
                sale => sale.Value,
                value => Sale.From(value).Value);
        builder.Property(p => p.MainImage)
            .HasConversion(value => value.Value,
                value => Image.From(value).Value);
    }
}

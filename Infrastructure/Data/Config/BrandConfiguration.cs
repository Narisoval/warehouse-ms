using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable("Brands");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name)
            .HasConversion(
                name => name.Value,
                value => BrandName.From(value).Value)
            .HasColumnName("Name");

        builder.Property(b => b.Image)
            .HasConversion(
                img => img.Value,
                value => Image.From(value).Value)
            .HasColumnName("Image");

        builder.Property(b => b.Description)
            .HasConversion(
                description => description.Value,
                value => BrandDescription.From(value).Value)
            .HasColumnName("Description");
    }
}
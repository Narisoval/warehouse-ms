using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable("ProductImages");
        
        builder.HasKey("Id","ProductId");
        
        builder.HasOne(pi => pi.Product)
            .WithMany(p => p.Images)
            .OnDelete(DeleteBehavior.Cascade); 
        
        builder.Property(productImage => productImage.Image)
            .HasConversion(
                image => image.Value,
                value => Image.From(value))
            .HasColumnName("Image");
    }
}
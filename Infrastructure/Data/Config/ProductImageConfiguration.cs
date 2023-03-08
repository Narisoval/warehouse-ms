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
        
        builder.Property(productImage => productImage.Image)
            .HasConversion(
                image => image.Value,
                value => Image.From(value).Value)
            .HasColumnName("Image");
        
        builder.HasKey("Image");
        
        builder.HasOne(pi => pi.Product)
            .WithMany(pr => pr.Images)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
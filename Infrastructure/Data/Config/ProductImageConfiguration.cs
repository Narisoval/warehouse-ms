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
        
        //builder.WithOwner().HasForeignKey("ProductId");
        
        builder.HasKey("Id","ProductId");
        
        builder.Property(productImage => productImage.Image)
            .HasConversion(
                image => image.Value,
                value => Image.From(value))
            .HasColumnName("Image");
    }
}
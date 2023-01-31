using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config;

public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.ToTable("Providers");
        builder.HasKey(x => x.Id);
             
        builder.Property(provider => provider.CompanyName)
            .HasConversion(companyName => companyName.Value,
                value => CompanyName.From(value));   
        
        builder.Property(provider => provider.Email)
            .HasConversion(email => email.Value,
            value => Email.From(value));   
}
}
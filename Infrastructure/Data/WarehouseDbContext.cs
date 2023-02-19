using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class WarehouseDbContext : DbContext
{
    private readonly List<Brand> _testBrands = new();
    private readonly List<Category> _testCategories = new();
    private readonly List<Provider> _testProviders = new();
    private readonly List<Product> _testProducts = new();

    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options)
    {
        ;
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WarehouseDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    private void PopulateTestBrands()
    {
        _testBrands.Add(Brand.Create(
            BrandName.From("Happy socks"),
            Image.From("https://happysocks.png"),
            BrandDescription.From("These socks are so happy, you won't believe it!") 
            ));
        
        _testBrands.Add(Brand.Create(
            BrandName.From("Happy cocks"),
            Image.From("https://happycocks.png"),
            BrandDescription.From("These cocks are so happy, you won't believe it!")
        ));

        _testBrands.Add(Brand.Create(
            BrandName.From("Happy dogs"),
            Image.From("https://happydogs.png"),
            BrandDescription.From("These dogs are so happy, you won't believe it!")
        ));
    }

    private void PopulateTestCategories()
    {
        
        _testCategories.Add(Category.Create(
        CategoryName.From("Socks")
        ));
        
        _testCategories.Add(Category.Create(
        CategoryName.From("Cocks")
        ));
        
        _testCategories.Add(Category.Create(
        CategoryName.From("Dogs")
        ));
    }

    private void PopulateTestProviders()
    {
        _testProviders.Add(Provider.Create(
            CompanyName.From("The PROVIDER THAT PROVIDES ANYTHING YOU WANT Inc."),
            "+124923503405",
            Email.From("provider@gmail.com")));    
    }

    private void PopulateTestProducts()
    {
        _testProducts.Add(Product.Create(
            ProductName.From("Happy socks socks XL"),
            Quantity.From(300),
            Price.From(400),
            null,
            ProductDescription.From("This product looks so good on you!!"),
            true,
            Sale.From(30),
            _testProviders[0].Id,
            _testBrands[0].Id,
            _testCategories[0].Id
        ));
        
        _testProducts.Add(Product.Create(
            ProductName.From("Happy socks white socks L"),
            Quantity.From(200),
            Price.From(350),
           null,
            ProductDescription.From("You'll love these socks just as much as the XL version!"),
            true,
            Sale.From(20),
            _testProviders[0].Id,
            _testBrands[0].Id,
            _testCategories[0].Id
        ));    
        
        _testProducts.Add(Product.Create(
            ProductName.From("Sad black cock"),
            Quantity.From(300),
            Price.From(400),
           null,
            ProductDescription.From("This is a really happy cock. You'll have lots of fun eating it"),
            true,
            Sale.From(15),
            _testProviders[0].Id,
            _testBrands[1].Id,
            _testCategories[1].Id
        ));
        
        _testProducts.Add(Product.Create(
            ProductName.From("A really happy black dog"),
            Quantity.From(100),
            Price.From(50000),
           null,
            ProductDescription.From("This is a really sad dog. That is because it doesn't have an owner"),
            true,
            Sale.From(15),
            _testProviders[0].Id,
            _testBrands[2].Id,
            _testCategories[2].Id
        ));
    }

    private IList<ProductImage> GetTestProductImages(string name)
    {
        List<ProductImage> images = new();

        images.Add(
            ProductImage.Create(Image.From($"https://{name}1.png"), true));

        images.Add(
            ProductImage.Create(Image.From($"https://{name}2.png"), false));
        
        images.Add(
            ProductImage.Create(Image.From($"https://{name}3.png"), false));
        
        return images;
    }
}
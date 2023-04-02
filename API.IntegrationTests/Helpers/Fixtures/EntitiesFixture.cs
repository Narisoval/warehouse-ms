using Domain.Entities;
using Domain.Primitives;
using Domain.ValueObjects;

namespace API.IntegrationTests.Helpers.Fixtures;

public static class EntitiesFixture
{
    #region TestData 
    
    public static List<Category> Categories { get; } = new()
    {
        Category.Create(Guid.NewGuid(), CategoryName.From("Socks").Value).Value,
        Category.Create(Guid.NewGuid(), CategoryName.From("Dogs").Value).Value,
        Category.Create(Guid.NewGuid(), CategoryName.From("Cocks").Value).Value
    };

    public static List<Brand> Brands { get; } = new()
    {
        Brand.Create(
            Guid.NewGuid(),
            BrandName.From("Happy Socks").Value,
            Image.From(
                    "https://img.favpng.com/10/3/23/happy-socks-men-s-cat-men-s-leopard-sock-s-socks-blue-pink-happy-socks-logo-png-favpng-NUf1FhWYaMTHq75Gf3LgJcXgp.jpg")
                .Value,
            BrandDescription.From("These socks are as happy as you will be while wearing them").Value).Value,

        Brand.Create(
            Guid.NewGuid(),
            BrandName.From("Happy Dogs").Value,
            Image.From(
                    "https://happydogclipsandkennels.files.wordpress.com/2019/08/happy-dog-country-clips-and-kennels-happy-dog-photos-1.png")
                .Value,
            BrandDescription.From("These dogs are so happy and these creatures will make you as happy as they are!")
                .Value).Value,

        Brand.Create(
            Guid.NewGuid(),
            BrandName.From("Happy Cocks").Value,
            Image.From(
                    "https://static.miraheze.org/avidwiki/thumb/7/7b/Happy_Cock_%281985%29_%28Credit_-_ifrequire%29.png/397px-Happy_Cock_%281985%29_%28Credit_-_ifrequire%29.png")
                .Value,
            BrandDescription.From("These cocks were really happy and now you're eating them!").Value).Value
    };

    public static List<Provider> Providers { get; } = new()
    {
        Provider.Create(
            Guid.NewGuid(),
            CompanyName.From("Socks owner inc").Value,
            PhoneNumber.From("+3806935093").Value,
            Email.From("dogs@gmail.com").Value).Value,

        Provider.Create(
            Guid.NewGuid(),
            CompanyName.From("Animals owner inc").Value,
            PhoneNumber.From("+38069344444").Value,
            Email.From("animals@gmail.com").Value).Value,
    };

    public static List<Product> Products { get; } = new()
    {
        Product.Create(
            id: Guid.NewGuid(),
            productName: ProductName.From("Happy men socks XL white color").Value,
            quantity: Quantity.From(300).Value,
            fullPrice: Price.From(200).Value,
            mainImage: Image.From("https://rainforestreports.weebly.com/uploads/1/5/5/7/15578800/9175403_orig.jpg")
                .Value,
            images: new List<ProductImage>
            {
                ProductImage
                    .Create(Image
                     .From(
                            "https://vignette.wikia.nocookie.net/naturerules1/images/b/b4/Capybara_nationalgeographic_1518115_2.ngsversion.1538507728041.adapt.1900.1.jpg")
                        .Value).Value,
            },
            productDescription: ProductDescription
                .From("100% cotton 1000% love. This socks will make you feel like no others").Value,
            isActive: true,
            sale: Sale.From(15).Value,
            providerId: Providers[0].Id,
            brandId: Brands[0].Id,
            categoryId: Categories[0].Id).Value,

        Product.Create(
            id: Guid.NewGuid(),
            productName: ProductName.From("Cozy women socks L black color").Value,
            quantity: Quantity.From(200).Value,
            fullPrice: Price.From(150).Value,
            mainImage: Image.From("https://cdn.shopify.com/s/files/1/0207/3802/products/cozy-socks-black-women.jpg")
                .Value,
            images: new List<ProductImage>
            {
                ProductImage
                    .Create(Image
                        .From("https://cdn.shopify.com/s/files/1/0207/3803/products/cozy-socks-black-women.jpg").Value)
                    .Value,
                ProductImage
                    .Create(Image
                        .From("https://cdn.shopify.com/s/files/1/0207/3802/products/cozy-socks-black-women-1.jpg")
                        .Value).Value
            },
            productDescription: ProductDescription
                .From("These socks will keep your feet warm and comfortable during the cold winter nights").Value,
            isActive: true,
            sale: Sale.From(10).Value,
            providerId: Providers[0].Id,
            brandId: Brands[0].Id,
            categoryId: Categories[0].Id).Value,

        Product.Create(
            id: Guid.NewGuid(),
            productName: ProductName.From("A black stray dog").Value,
            quantity: Quantity.From(2).Value,
            fullPrice: Price.From(10).Value,
            mainImage: Image
                .From(
                    "https://gumlet.assettype.com/freepressjournal%2F2020-08%2F7bdd9149-37ea-4e9e-b1b8-0456f0d9d7bd%2F6132036Iq4DASOT.jpg")
                .Value,
            images: new List<ProductImage>
            {
                ProductImage.Create(Image.From("https://1721181113.rsc.cdn77.org/data/images/full/26936/stray-dogs.jpg")
                    .Value).Value,
            },
            productDescription: ProductDescription
                .From("This stray dog is probably the cutest thing you'll ever see in your life").Value,
            isActive: true,
            sale: Sale.From(0).Value,
            providerId: Providers[1].Id,
            brandId: Brands[1].Id,
            categoryId: Categories[1].Id).Value,

        Product.Create(
            id: Guid.NewGuid(),
            productName: ProductName.From("White French bulldog").Value,
            quantity: Quantity.From(50).Value,
            fullPrice: Price.From(250).Value,
            mainImage: Image
                .From(
                    "https://www.allfrbulldogs.com/wp-content/uploads/2021/03/How-to-Care-For-White-French-Bulldog.jpg")
                .Value,
            images: new List<ProductImage>
            {
                ProductImage
                    .Create(Image
                        .From("https://upload.wikimedia.org/wikipedia/commons/7/76/Altman_White_English_Bulldog.jpg")
                        .Value).Value,
                ProductImage
                    .Create(Image
                        .From(
                            "https://www.publicdomainpictures.net/pictures/290000/velka/white-english-bulldog-portrait.jpg")
                        .Value).Value
            },
            productDescription: ProductDescription.From("This white bulldog will love you like your dad never did")
                .Value,
            isActive: true,
            sale: Sale.From(15).Value,
            providerId: Providers[1].Id,
            brandId: Brands[1].Id,
            categoryId: Categories[1].Id).Value,

        Product.Create(
            id: Guid.NewGuid(),
            productName: ProductName.From("Male cock with Red Comb and Feathers").Value,
            quantity: Quantity.From(50).Value,
            fullPrice: Price.From(30).Value,
            mainImage: Image.From("https://example.com/rooster1.jpg").Value,
            images: new List<ProductImage>
            {
                ProductImage.Create(Image.From("https://example.com/rooster1_1.jpg").Value).Value,
                ProductImage.Create(Image.From("https://example.com/rooster1_2.jpg").Value).Value,
                ProductImage.Create(Image.From("https://example.com/rooster1_3.jpg").Value).Value
            },
            productDescription: ProductDescription
                .From(
                    "This beautiful male rooster has bright red feathers and a comb. It would make a great addition to any farm or backyard flock.")
                .Value,
            isActive: true,
            sale: Sale.From(0).Value,
            providerId: Providers[1].Id,
            brandId: Brands[2].Id,
            categoryId: Categories[2].Id).Value,

        Product.Create(
            id: Guid.NewGuid(),
            productName: ProductName.From("Beautiful cock with green feathers").Value,
            quantity: Quantity.From(50).Value,
            fullPrice: Price.From(100).Value,
            mainImage: Image.From("https://example.com/rooster-green.jpg").Value,
            images: new List<ProductImage>
            {
                ProductImage.Create(Image.From("https://example.com/rooster-green-1.jpg").Value).Value,
                ProductImage.Create(Image.From("https://example.com/rooster-green-2.jpg").Value).Value,
                ProductImage.Create(Image.From("https://example.com/rooster-green-3.jpg").Value).Value
            },
            productDescription: ProductDescription
                .From(
                    "This beautiful cock has vibrant green feathers and a proud stance. He would make a perfect addition to any farm or backyard flock.")
                .Value,
            isActive: true,
            sale: Sale.From(10).Value,
            providerId: Providers[1].Id,
            brandId: Brands[2].Id,
            categoryId: Categories[2].Id).Value
    };
    
    #endregion
    
    public static T GetRandomEntity<T>() where T : Entity
    {
        Random rnd = new Random();
        var entityList = GetList<T>();
        
        var randomIndex = rnd.Next(entityList.Count);
        return entityList[randomIndex];
    }
    
    public static List<T> GetList<T>() where T : Entity
    {
        if (typeof(T) == typeof(Category))
        {
            return Categories.Cast<T>().ToList();
        }

        if (typeof(T) == typeof(Brand))
        {
            return Brands.Cast<T>().ToList();
        }

        if (typeof(T) == typeof(Provider))
        {
            return Providers.Cast<T>().ToList();
        }

        if (typeof(T) == typeof(Product))
        {
            return Products.Cast<T>().ToList();
        }

        throw new InvalidOperationException($"Invalid type: {typeof(T).Name}");
    }
}
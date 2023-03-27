using Warehouse.API.Messaging.Events;
using Warehouse.API.Messaging.Events.BrandEvents;
using Warehouse.API.Messaging.Events.CategoryEvents;
using Warehouse.API.Messaging.Events.ProductEvents;

namespace Infrastructure.IntegrationTests.Helpers.Fixtures;

public static class EventsFixture
{
    private static Guid TestId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6");

    public static T Get<T>() where T : class, IEvent
    {
        if (typeof(T) == typeof(BrandCreatedEvent))
            return (BrandCreatedEventExample as T)!;
        
        if (typeof(T) == typeof(BrandUpdatedEvent))
            return (BrandUpdatedEventExample as T)!;

        if (typeof(T) == typeof(BrandDeletedEvent))
            return (BrandDeletedEventExample as T)!;
        

        if (typeof(T) == typeof(CategoryCreatedEvent))
            return (CategoryCreatedEventExample as T)!;
        
        if (typeof(T) == typeof(CategoryUpdatedEvent))
            return (CategoryUpdatedEventExample as T)!;
        
        if (typeof(T) == typeof(CategoryDeletedEvent))
            return (CategoryDeletedEventExample as T)!;

        
        if (typeof(T) == typeof(ProductCreatedEvent))
            return (ProductCreatedEventExample as T)!;
        
        if (typeof(T) == typeof(ProductUpdatedEvent))
            return (ProductUpdatedEventExample as T)!;
            
        if (typeof(T) == typeof(ProductDeletedEvent))
            return (ProductDeletedEventExample as T)!;
        
        throw new InvalidOperationException($"Invalid event type {typeof(T)}");
    }

    #region BrandEventsExamples
    
    private static readonly BrandCreatedEvent BrandCreatedEventExample = new BrandCreatedEvent
    {
        Id = TestId,
        Name = "TestBrand",
        Description = "Test brand description for test brand",
        Image = "https://testimagesstore.com/testimage.png"
    };
    
    private static readonly BrandUpdatedEvent BrandUpdatedEventExample = new BrandUpdatedEvent
    {
        Id = TestId,
        Name = "TestBrand",
        Description = "Test brand description for test brand",
        Image = "https://testimagesstore.com/testimage.png"
    };

    private static readonly BrandDeletedEvent BrandDeletedEventExample = new BrandDeletedEvent(TestId);
    
    #endregion
    
    #region CategoryEventsExamples
    
    private static readonly CategoryCreatedEvent CategoryCreatedEventExample = new CategoryCreatedEvent
    {
        Id = TestId,
        Name = "Test category!!"
    };

    private static readonly CategoryUpdatedEvent CategoryUpdatedEventExample = new CategoryUpdatedEvent
    {
        Id = TestId,
        Name = "Test category!!"
    };

    private static readonly CategoryDeletedEvent CategoryDeletedEventExample = new CategoryDeletedEvent(TestId);
    
    #endregion

    #region ProductEventExamples

    private static ProductCreatedEvent ProductCreatedEventExample = new ProductCreatedEvent
    {
        Id = TestId,
        Name = "Test product",
        Description = "This test product will be tested real hard!",
        MainImage = "https://testimagesstore.com/testproductImage.png",
        Images = new List<string>
        {
            "https://testimagesstore.com/testproductImage-1.png",
            "https://testimagesstore.com/testproductImage-1.png",
        },
        FullPrice = 30m,
        Discount = 10,
        Quantity = 400,
        IsActive = true,
        CategoryId = TestId,
        BrandId = TestId,
    };

    private static ProductUpdatedEvent ProductUpdatedEventExample = new ProductUpdatedEvent
    {
        Id = TestId,
        Name = "Test product",
        Description = "This test product will be tested real hard!",
        MainImage = "https://testimagesstore.com/testproductImage.png",
        Images = new List<string>
        {
            "https://testimagesstore.com/testproductImage-1.png",
            "https://testimagesstore.com/testproductImage-1.png",
        },
        FullPrice = 30m,
        Discount = 10,
        Quantity = 400,
        IsActive = true,
        CategoryId = TestId,
        BrandId = TestId,
    };

    private static ProductDeletedEvent ProductDeletedEventExample = new ProductDeletedEvent(TestId);

    #endregion

}
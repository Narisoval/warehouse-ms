using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.ProductEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.ProductEventsTests;

public class ProductDeletedEventTests : BaseEventTest<ProductDeletedEvent>
{
    public ProductDeletedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.ProductEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.ProductEventsTests;

public class ProductUpdatedEventTests : BaseEventTest<ProductUpdatedEvent>
{
    public ProductUpdatedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
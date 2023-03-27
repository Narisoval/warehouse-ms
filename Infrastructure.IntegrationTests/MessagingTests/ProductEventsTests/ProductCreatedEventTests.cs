using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.ProductEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.ProductEventsTests;

public class ProductCreatedEventTests : BaseEventTest<ProductCreatedEvent>
{
    public ProductCreatedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
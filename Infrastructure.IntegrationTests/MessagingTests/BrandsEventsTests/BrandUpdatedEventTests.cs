using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.BrandEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.BrandsEventsTests;

public class BrandUpdatedEventTests : BaseEventTest<BrandUpdatedEvent>
{
    public BrandUpdatedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
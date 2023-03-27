using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.BrandEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.BrandsEventsTests;

public class BrandsCreatedEventTests : BaseEventTest<BrandCreatedEvent>
{
    public BrandsCreatedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
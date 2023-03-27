using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.BrandEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.BrandsEventsTests;

public class BrandDeletedEventTests : BaseEventTest<BrandDeletedEvent>
{
    public BrandDeletedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.CategoryEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.CategoryEventsTests;

public class CategoryUpdatedTests : BaseEventTest<CategoryUpdatedEvent>
{
    public CategoryUpdatedTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
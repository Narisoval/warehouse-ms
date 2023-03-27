using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.CategoryEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.CategoryEventsTests;

public class CategoryDeletedTests : BaseEventTest<CategoryDeletedEvent>
{
    public CategoryDeletedTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
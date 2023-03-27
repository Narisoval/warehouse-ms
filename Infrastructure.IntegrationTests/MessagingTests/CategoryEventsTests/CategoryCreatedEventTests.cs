using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Warehouse.API.Messaging.Events.CategoryEvents;

namespace Infrastructure.IntegrationTests.MessagingTests.CategoryEventsTests;

public class CategoryCreatedEventTests : BaseEventTest<CategoryCreatedEvent>
{
    public CategoryCreatedEventTests(MessageBusFixture fixture) : base(fixture)
    {
    }
}
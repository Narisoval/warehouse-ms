using FluentAssertions;
using Infrastructure.IntegrationTests.Helpers.Fixtures;
using Infrastructure.MessageBroker.EventBus;
using MassTransit.Testing;
using Warehouse.API.Messaging.Events;

namespace Infrastructure.IntegrationTests.MessagingTests;


public abstract class BaseEventTest<T> : IClassFixture<MessageBusFixture>
where T : class, IEvent
{
    private readonly ITestHarness _testHarness;
    private readonly IEventBus _eventBus;
    private readonly SemaphoreSlim _semaphore;
    public BaseEventTest(MessageBusFixture fixture)
    {
        _testHarness = fixture.GetTestHarness();
        _eventBus = fixture.GetEventBus();
        _semaphore = new SemaphoreSlim(1, 1);
    }

    [Fact]
    public async Task Should_PublishMessageSuccessfully_WhenPublishMessageAsync()
    {
        // Arrange
        await _testHarness.Start();
        var eventToPublish = EventsFixture.Get<T>();

        // Act
        var messagesPublished = await GetPublishedMessagesDifference
            (async () => await _eventBus.PublishAsync(eventToPublish));

        messagesPublished.Should().Be(1);
        var publishedMessages = _testHarness.Published.Select<T>().ToList();
        publishedMessages[^1].MessageObject.Should().BeEquivalentTo(eventToPublish);
    }
    
    [Fact]
    public async Task Should_NotPublishMessage_WhenCancellingWithCancellationToken()
    {
        // Arrange
        await _testHarness.Start();
        var eventToPublish = EventsFixture.Get<T>();
        
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();
        var token = cancellationTokenSource.Token;
        
        // Act
        var eventPublishing = async () => await _eventBus.PublishAsync(eventToPublish, token);
        
        // Assert
        var messagesPublished =  
            await GetPublishedMessagesDifference(
                async () => await eventPublishing.Should().ThrowExactlyAsync<OperationCanceledException>());
        
        messagesPublished.Should().Be(0);
    }
    
    private async Task<int> GetPublishedMessagesDifference(Func<Task> method)
    {
        await _semaphore.WaitAsync();
        try
        {
            var publishedMessagesCountBefore = _testHarness.Published.Select<T>().Count();
            await method();
            var publishedMessagesAfter = _testHarness.Published.Select<T>().Count();
            
            return publishedMessagesAfter - publishedMessagesCountBefore;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}